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
                                            && x.PatientId == "")
                                .ToList();

            if (workerId == "")
            {
                appointments = appointments
                                .Where(x => x.MedicalWorkerId == workerId)
                                .ToList();
            }
            return View(appointments);
        }

        // GET: Appointments/GetAllAppointments/5?
        [HttpGet]
        [Route("[controller]/[action]/{pharmacyId?}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments(string pharmacyId = "")
        {
            if(pharmacyId == "")
                return View(_context.Appointment.ToList());

            return View(_context.Appointment
                                .Where(x => x.PharmacyId.ToString() == pharmacyId)
                                .ToList());
        }

        // GET: Appointments/Details/5
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

        // GET: Appointments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
