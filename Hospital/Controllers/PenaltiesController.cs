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
    public class PenaltiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PenaltiesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Penalties
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if(User.IsInRole("Admin"))
                return View(await _context.Penalty.ToListAsync());

            var userId = (await _userManager.GetUserAsync(User)).Id;

            return View(await _context.Penalty.Where(x => x.PatientId == userId).ToListAsync());
        }

        // GET: Penalties/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penalty = await _context.Penalty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (penalty == null)
            {
                return NotFound();
            }

            return View(penalty);
        }

        // GET: Penalties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Penalties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,Value,Id,RowVersion")] Penalty penalty)
        {
            if (ModelState.IsValid)
            {
                penalty.Id = Guid.NewGuid();
                _context.Add(penalty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(penalty);
        }

        // GET: Penalties/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penalty = await _context.Penalty.FindAsync(id);
            if (penalty == null)
            {
                return NotFound();
            }
            return View(penalty);
        }

        // POST: Penalties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PatientId,Value,Id,RowVersion")] Penalty penalty)
        {
            if (id != penalty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(penalty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PenaltyExists(penalty.Id))
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
            return View(penalty);
        }

        // GET: Penalties/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penalty = await _context.Penalty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (penalty == null)
            {
                return NotFound();
            }

            return View(penalty);
        }

        // POST: Penalties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var penalty = await _context.Penalty.FindAsync(id);
            _context.Penalty.Remove(penalty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PenaltyExists(Guid id)
        {
            return _context.Penalty.Any(e => e.Id == id);
        }
    }
}
