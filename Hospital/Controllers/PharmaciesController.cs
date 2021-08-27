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
using System.Text.RegularExpressions;

namespace Hospital.Controllers
{
    public class PharmaciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PharmaciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pharmacies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pharmacy.ToListAsync());
        }

        // GET: Pharmacies/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacy = await _context.Pharmacy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            //set hotel location to proper url format
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            ViewData["Address"] = rgx.Replace(pharmacy.Location, "").Replace(" ", "%20");

            if (_context.Review
                .Where(x => x.ReviewedId == pharmacy.Id.ToString()).ToList().Count == 0)
            {
                ViewData["Rating"] = 0;
                ViewData["RatingsCount"] = 0;
            }
            else
            {
                //dynamically calculated field: pharmacy rating
                ViewData["Rating"] = Math.Round(_context.Review
                                            .Where(x => x.ReviewedId == pharmacy.Id.ToString())
                                            .Select(x => x.Rating)
                                            .ToList().Average(), 2);
                ViewData["RatingsCount"] = _context.Review
                                            .Where(x => x.ReviewedId == pharmacy.Id.ToString())
                                            .Count();
            }

            return View(pharmacy);
        }

        // GET: Pharmacies/Create
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pharmacies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,RowVersion")] Pharmacy pharmacy)
        {
            if (ModelState.IsValid)
            {
                pharmacy.Id = Guid.NewGuid();
                _context.Add(pharmacy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pharmacy);
        }

        // GET: Pharmacies/Edit/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacy = await _context.Pharmacy.FindAsync(id);
            if (pharmacy == null)
            {
                return NotFound();
            }
            return View(pharmacy);
        }

        // POST: Pharmacies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Location,RowVersion")] Pharmacy pharmacy)
        {
            if (id != pharmacy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pharmacy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PharmacyExists(pharmacy.Id))
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
            return View(pharmacy);
        }

        // GET: Pharmacies/Delete/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacy = await _context.Pharmacy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            return View(pharmacy);
        }

        // POST: Pharmacies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pharmacy = await _context.Pharmacy.FindAsync(id);
            _context.Pharmacy.Remove(pharmacy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PharmacyExists(Guid id)
        {
            return _context.Pharmacy.Any(e => e.Id == id);
        }
    }
}
