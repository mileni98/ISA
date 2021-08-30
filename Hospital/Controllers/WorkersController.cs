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
using Hospital.Models.DTO;

namespace Hospital.Controllers
{
    public class WorkersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WorkersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: 
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            var medWorkers = new List<WorkerDTO>();
            foreach(var user in users)
            {
                if((await _userManager.IsInRoleAsync(user,"Doctor")) 
                    || await _userManager.IsInRoleAsync(user, "Pharmacist"))
                {
                    WorkerDTO worker = new WorkerDTO();
                    worker.Name = user.UserName;
                    worker.Id = user.Id;
                    
                    if(_context.Review
                        .Where(x => x.ReviewedId == user.Id.ToString()).ToList().Count == 0)
                    {
                        worker.Rating = 0;
                        worker.NumberOfRatings = 0;
                    }
                    else
                    {
                        worker.Rating = 
                                            Math.Round(_context.Review
                                            .Where(x => x.ReviewedId == user.Id.ToString())
                                            .Select(x => x.Rating)
                                            .ToList().Average(), 2); ;
                        worker.NumberOfRatings =
                                            _context.Review
                                            .Where(x => x.ReviewedId == user.Id.ToString())
                                            .Count();
                    }
                    
                    worker.Profession = (await _userManager.IsInRoleAsync(user, "Pharmacist")) ? "Pharmacist" : "Doctor";
                    
                    medWorkers.Add(worker);
                }
            }

            return View(medWorkers);
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if ((await _userManager.IsInRoleAsync(user, "Doctor"))
                    || await _userManager.IsInRoleAsync(user, "Pharmacist"))
            {
                WorkerDTO worker = new WorkerDTO();
                worker.Name = user.UserName;
                worker.Id = user.Id;

                if (_context.Review
                    .Where(x => x.ReviewedId == user.Id.ToString()).ToList().Count == 0)
                {
                    worker.Rating = 0;
                    worker.NumberOfRatings = 0;
                }
                else
                {
                    worker.Rating =
                                        Math.Round(_context.Review
                                        .Where(x => x.ReviewedId == user.Id.ToString())
                                        .Select(x => x.Rating)
                                        .ToList().Average(), 2); ;
                    worker.NumberOfRatings =
                                        _context.Review
                                        .Where(x => x.ReviewedId == user.Id.ToString())
                                        .Count();
                }

                worker.Profession = (await _userManager.IsInRoleAsync(user, "Pharmacist")) ? "Pharmacist" : "Doctor";
                return View(worker);
            }

            return NotFound();
        }

        private bool WorkerExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
