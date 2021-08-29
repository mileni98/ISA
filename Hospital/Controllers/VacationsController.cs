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
using Microsoft.AspNetCore.Identity;

namespace Hospital.Controllers
{
    public class VacationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VacationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Vacations
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> Index()
        {
            if(User.IsInRole("Admin"))
                return View(await _context.Vacation.ToListAsync());

            var id = (await _userManager.GetUserAsync(User)).Id;

            return View(await _context.Vacation.Where(x => x.WorkerId == id).ToListAsync());
        }

        // GET: Vacations/Details/5
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacation = await _context.Vacation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacation == null)
            {
                return NotFound();
            }

            return View(vacation);
        }

        // GET: Vacations/Create
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> Create()
        {
            var vacation = new Vacation();
            vacation.WorkerId = (await _userManager.GetUserAsync(User)).Id;
            return View(vacation);
        }

        // POST: Vacations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> Create([Bind("WorkerId,StartTime,EndTime,CreationTime,IsApproved,ApprovalTime,Id,RowVersion")] Vacation vacation)
        {
            if (ModelState.IsValid)
            {
                vacation.Id = Guid.NewGuid();
                _context.Add(vacation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vacation);
        }

        // GET: Vacations/Edit/5
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacation = await _context.Vacation.FindAsync(id);
            if (vacation == null)
            {
                return NotFound();
            }

            if(vacation.HasBeenReviewed)
            {
                ViewData["errorMessage"] = "Vacation request has already been reviewed";
                return View("ErrorMessage");
            }

            return View(vacation);
        }

        // POST: Vacations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> Edit(Guid id, [Bind("WorkerId,StartTime,EndTime,CreationTime,IsApproved,ApprovalTime,Id,RowVersion,HasBeenReviewed")] Vacation vacation)
        {
            if (id != vacation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (vacation.HasBeenReviewed)
                        vacation.ApprovalTime = DateTime.Now;

                    _context.Update(vacation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationExists(vacation.Id))
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
            return View(vacation);
        }

        // GET: Vacations/Delete/5
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacation = await _context.Vacation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacation == null)
            {
                return NotFound();
            }

            return View(vacation);
        }

        // POST: Vacations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vacation = await _context.Vacation.FindAsync(id);
            _context.Vacation.Remove(vacation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacationExists(Guid id)
        {
            return _context.Vacation.Any(e => e.Id == id);
        }
    }
}
