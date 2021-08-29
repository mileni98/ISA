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

namespace Hospital.Controllers
{
    public class DrugsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DrugsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Drugs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Drug.ToListAsync());
        }

        // GET: Drugs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drug = await _context.Drug
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drug == null)
            {
                return NotFound();
            }

            return View(drug);
        }

        // GET: Drugs/Create
        [Authorize(Roles = "Admin,Pharmacist")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drugs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Pharmacist")]
        public async Task<IActionResult> Create([Bind("Name,Description,Ingredients,Notes,Id,RowVersion")] Drug drug)
        {
            if (ModelState.IsValid)
            {
                drug.Id = Guid.NewGuid();
                _context.Add(drug);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drug);
        }

        // GET: Drugs/Edit/5
        [Authorize(Roles = "Admin,Pharmacist")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drug = await _context.Drug.FindAsync(id);
            if (drug == null)
            {
                return NotFound();
            }
            return View(drug);
        }

        // POST: Drugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Pharmacist")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Description,Ingredients,Notes,Id,RowVersion")] Drug drug)
        {
            if (id != drug.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drug);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrugExists(drug.Id))
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
            return View(drug);
        }

        // GET: Drugs/Delete/5
        [Authorize(Roles = "Admin,Pharmacist")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drug = await _context.Drug
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drug == null)
            {
                return NotFound();
            }

            return View(drug);
        }

        // POST: Drugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Pharmacist")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var drug = await _context.Drug.FindAsync(id);
            _context.Drug.Remove(drug);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrugExists(Guid id)
        {
            return _context.Drug.Any(e => e.Id == id);
        }
    }
}
