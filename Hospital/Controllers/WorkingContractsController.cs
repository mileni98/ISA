using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.Models;
using Microsoft.AspNetCore.Authorization;
using Hospital.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Controllers
{
    public class WorkingContractsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WorkingContractsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: WorkingContracts
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
                return View(await _context.WorkingContract.ToListAsync());

            var id = (await _userManager.GetUserAsync(User)).Id;

            return View(await _context.WorkingContract.Where(x => x.WorkerId == id).ToListAsync());
        }

        // GET: WorkingContracts/Details/5
        [Authorize(Roles = "Admin,Doctor,Pharmacist")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingContract = await _context.WorkingContract
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workingContract == null)
            {
                return NotFound();
            }

            ViewData["pharmacyName"] = _context.Pharmacy.ToList()
                                        .Where(x => x.Id == workingContract.PharmacyId)
                                        .Select(x => x.Name).FirstOrDefault();
            ViewData["workerUsername"] = _context.Users
                                        .Where(x => x.Id == workingContract.WorkerId)
                                        .Select(x => x.UserName).FirstOrDefault();

            return View(workingContract);
        }

        // GET: WorkingContracts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["pharmacies"] = _context.Pharmacy.ToList();
            ViewData["workers"] = _context.Users.ToList();
            return View();
        }

        // POST: WorkingContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("WorkerId,PharmacyId,StartTime,EndTime,WorkTimeStart,WorkTimeEnd,Id,RowVersion,WorkingTimeStart,WorkingTimeEnd")] WorkingContractDTO workingContractDto)
        {
            var workingContract = new WorkingContract(workingContractDto);
            if (ModelState.IsValid)
            {
                workingContract.Id = Guid.NewGuid();
                _context.Add(workingContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workingContract);
        }

        // GET: WorkingContracts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingContract = await _context.WorkingContract.FindAsync(id);
            if (workingContract == null)
            {
                return NotFound();
            }
            return View(workingContract);
        }

        // POST: WorkingContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("WorkerId,PharmacyId,StartTime,EndTime,WorkTimeStart,WorkTimeEnd,Id,RowVersion,WorkingTimeStart,WorkingTimeEnd")] WorkingContractDTO workingContractDto)
        {
            var workingContract = new WorkingContract(workingContractDto);
            if (id != workingContract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workingContract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkingContractExists(workingContract.Id))
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
            return View(workingContract);
        }

        // GET: WorkingContracts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingContract = await _context.WorkingContract
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workingContract == null)
            {
                return NotFound();
            }

            return View(workingContract);
        }

        // POST: WorkingContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var workingContract = await _context.WorkingContract.FindAsync(id);
            _context.WorkingContract.Remove(workingContract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkingContractExists(Guid id)
        {
            return _context.WorkingContract.Any(e => e.Id == id);
        }
    }
}
