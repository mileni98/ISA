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
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var id = (await _userManager.GetUserAsync(User)).Id;
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Reservation.ToListAsync());
            }
            return View(await _context.Reservation.Where(x => x.UserId == id).ToListAsync());
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reservation = await _context.Reservation.FindAsync(id);

            if(DateTime.Now > reservation.StartTime.AddDays(2))
            {
                //add penalty if reservation late
                var penalty = _context.Penalty.Where(x => x.PatientId == reservation.UserId).FirstOrDefault();

                if (penalty == null)
                {
                    penalty = new Penalty();
                    penalty.PatientId = reservation.UserId;
                    penalty.Value = 1;
                    _context.Add(penalty);
                }
                else
                {
                    penalty.Value++;
                }
            }
            
            //don't delete old reservation just close them
            //_context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(Guid id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
