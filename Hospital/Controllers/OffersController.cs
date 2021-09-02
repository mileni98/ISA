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
    public class OffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OffersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Offers
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Offer.ToListAsync());
        }

        // GET: Offers/Details/5
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // GET: Offers/Create
        [Authorize(Roles = "Admin,Supplier")]
        public IActionResult Create()
        {
            ViewData["drugs"] = _context.Drug.ToList();
            ViewData["pharmacies"] = _context.Pharmacy.ToList();
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> Create([Bind("CreationTime,OfferedById,DrugId,PharmacyId,CreatedByUserId,Id,RowVersion,IsClosed")] Offer offer)
        {
            offer.CreatedByUserId = (await _userManager.GetUserAsync(User)).Id;
            offer.OfferedById = "";

            if (ModelState.IsValid)
            {
                offer.Id = Guid.NewGuid();
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
        }

        // GET: Offers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("CreationTime,OfferedById,DrugId,PharmacyId,CreatedByUserId,Id,RowVersion,IsClosed")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                if(offer.IsClosed)
                {
                    Item item = new Item();
                    item.ItemId = offer.DrugId;
                    item.PharmacyId = offer.PharmacyId;
                    item.Price = 100;
                    item.IsAvailable = true;
                    _context.Add(item);
                    _context.Remove(offer);
                    _context.SaveChanges();

                    return RedirectToAction("Index","Items");
                }
                else
                {
                    try
                    {
                        _context.Update(offer);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!OfferExists(offer.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
        }


        // GET: Offers/Edit/5
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> Take(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> Take(Guid id, [Bind("CreationTime,OfferedById,DrugId,PharmacyId,CreatedByUserId,Id,RowVersion,IsClosed")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                offer.OfferedById = (await _userManager.GetUserAsync(User)).Id;
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
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
            return View(offer);
        }

        // GET: Offers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var offer = await _context.Offer.FindAsync(id);
            _context.Offer.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(Guid id)
        {
            return _context.Offer.Any(e => e.Id == id);
        }
    }
}
