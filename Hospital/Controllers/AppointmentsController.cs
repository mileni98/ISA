using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = (await _userManager.GetUserAsync(User));
            var appointments = await _context.Appointment
                                    .Where(x => x.MedicalWorkerId == user.Id || x.PatientId == user.Id)
                                    .ToListAsync();

            return View(appointments);
        }

        // GET: Appointments/GetFreeAppointments/
        [HttpGet]
        [Route("[controller]/[action]")]
        [Authorize]
        public async Task<IActionResult> GetFreeAppointments()
        {
            var appointments = _context.Appointment
                                .Where(x => x.PatientId == null)
                                .ToList();

            return View("Index", appointments);
        }

        // GET: Appointments/GetPharmaciesWithFreeAppointment
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetPharmaciesWithFreeAppointment()
        {
            ViewData["pharmacies"] = _context.Pharmacy.ToList();
            return View(new Appointment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateAppointmentRequest([Bind("PharmacyId,PatientId,MedicalWorkerId,StartTime,EndTime,Rating,Price,Comment,Id,RowVersion")] Appointment appointment)
        {
            //patients dont get to choose the length of the appointment
            appointment.EndTime = appointment.StartTime.AddMinutes(30);

            var medExpertsIds = _context.WorkingContract.
                Where(x => x.PharmacyId == appointment.PharmacyId)
                .Select(x => x.WorkerId)
                .ToList();

            var freeMedExperts = _context.Users
                                .Where(x => medExpertsIds.Contains(x.Id)).ToList();

            foreach(var medExpert in freeMedExperts.ToList())
            {
                var appointments = _context.Appointment
                                .Where(x => x.MedicalWorkerId == medExpert.Id)
                                .ToList();

                bool isFree = true;

                foreach(var app in appointments)
                {
                    if(IsOverlapping(app, appointment))
                    {
                        isFree = false;
                        break;
                    }
                }

                if (!isFree)
                    freeMedExperts.Remove(medExpert);

                if ((await _userManager.GetRolesAsync(medExpert)).FirstOrDefault() == "Admin")
                    freeMedExperts.Remove(medExpert);
            }

            if(freeMedExperts.Count == 0)
            {
                return View("ErrorMessage", "No free med experts for this appointment");
            }

            ViewData["freeMedExperts"] = freeMedExperts;
            appointment.PatientId = (await _userManager.GetUserAsync(User)).Id;

            return View("Create", appointment);
        }

        public bool IsOverlapping(Appointment a1, Appointment a2)
        {
            if (a1.StartTime <= a2.StartTime && a1.EndTime >= a2.EndTime)
                return true;
            if (a1.StartTime >= a2.StartTime && a1.EndTime <= a2.EndTime)
                return true;

            return false;
        }

        // GET: Appointments/GetFreeAppointments/5/5?
        [HttpGet]
        [Route("[controller]/[action]/{pharmacyId}/{workerId?}")]
        [Authorize]
        public async Task<IActionResult> GetFreeAppointments(string pharmacyId, string workerId = "")
        {
            if (pharmacyId == "")
                return View(_context.Appointment.ToList());

            var appointments = _context.Appointment
                                .Where(x => x.PharmacyId.ToString() == pharmacyId
                                            && x.PatientId == null)
                                .ToList();

            if (workerId == "")
            {
                appointments = appointments
                                .Where(x => x.MedicalWorkerId == workerId)
                                .ToList();
            }
            return View("Index", appointments);
        }

        // GET: Appointments/GetAllAppointments/5?
        [HttpGet]
        [Route("[controller]/[action]/{pharmacyId?}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments(string pharmacyId = "")
        {
            if(pharmacyId == "")
                return View("Index", _context.Appointment.ToList());

            return View("Index", _context.Appointment
                                .Where(x => x.PharmacyId.ToString() == pharmacyId)
                                .ToList());
        }

        // GET: Appointments/Details/5
        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/CreateForMedExpert/5
        [Authorize(Roles = "Admin,Doctor,Pharmacist")]
        public async Task<IActionResult> CreateForMedExpert(string id)
        {
            var medExpert = await _userManager.FindByIdAsync(id);

            if(!(await _userManager.IsInRoleAsync(medExpert, "Doctor")) &&
                !(await _userManager.IsInRoleAsync(medExpert, "Pharmacist")))
            {
                return View("ErrorMessage", "User is not a medical expert");
            }

            var pharmaciesWithContracts = await _context.WorkingContract
                                        .Where(x => x.WorkerId == id)
                                        .Select(x => x.PharmacyId)
                                        .ToListAsync();

            if (pharmaciesWithContracts.Count == 0)
            {
                return View("ErrorMessage", "Medical expert has no contracts.");
            }

            var pharmacies = await _context.Pharmacy
                                        .Where(x => pharmaciesWithContracts.Contains(x.Id))
                                        .ToListAsync();

            ViewData["expertId"] = id;
            ViewData["expertUsername"] = medExpert.UserName;
            ViewData["pharmacies"] = pharmacies;

            Appointment appointment = new Appointment();
            appointment.MedicalWorkerId = id;
            return View(appointment);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("PharmacyId,PatientId,MedicalWorkerId,StartTime,EndTime,Rating,Price,Comment,Id,RowVersion")] Appointment appointment)
        {
            //TODO: create free appointments for medexpert
            if (ModelState.IsValid)
            {
                appointment.Id = Guid.NewGuid();
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Authorize(Roles = "Admin,Doctor,Pharmacist")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/TakeAppointment/5
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> TakeAppointment(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);

            appointment.PatientId = (await _userManager.GetUserAsync(User)).Id;
            
            try
            {
                _context.Update(appointment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(appointment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return View("Successful");
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor,Pharmacist")]
        public async Task<IActionResult> Edit(Guid id, [Bind("PharmacyId,PatientId,MedicalWorkerId,StartTime,EndTime,Rating,Price,Comment,Id,RowVersion")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(Guid id)
        {
            return _context.Appointment.Any(e => e.Id == id);
        }
    }
}
