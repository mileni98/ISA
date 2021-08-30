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

        [HttpGet]
        [Route("[controller]/[action]/{pharmacyId}/{role}")]
        public async Task<IActionResult> WorkersForPharmacy(Guid pharmacyId, string role)
        {
            if(!_context.Pharmacy.Any(e => e.Id == pharmacyId))
            {
                return NotFound();
            }

            var roles = new List<string> { "Doctor", "Pharmacist" };
            if (role != null)
            {
                roles = new List<string> { role };
            }
            var workersWithContract = _context.WorkingContract.Where(x => x.PharmacyId == pharmacyId).Select(x => x.WorkerId).ToList();
            var workers = _context.Users.Where(x => workersWithContract.Contains(x.Id)).ToList();
            ViewData["pharmacyName"] = _context.Pharmacy.Where(x => x.Id == pharmacyId).Select(x => x.Name).FirstOrDefault();
            //TODO: Test
            return View("Index", ShowWorkers(roles, workers));
        }

        [HttpGet]
        [Route("[controller]/[action]/{role}")]
        public async Task<IActionResult> WorkersForRole(string role)
        {
            return View("Index", ShowWorkers(new List<string> { role }));
        }

        // GET: 
        public async Task<IActionResult> Index()
        {
            return View(ShowWorkers(new List<string> { "Doctor", "Pharmacist" }));
        }

        public List<WorkerDTO> ShowWorkers(List<string> Roles, List<IdentityUser> users = null)
        {
            if(users == null)
            {
                users = await _context.Users.ToListAsync();
            }
            var medWorkers = new List<WorkerDTO>();
            foreach(var user in users)
            {
                bool isInRole = false;
                foreach(var role in Roles)
                {
                    isInRole = isInRole || (await _userManager.IsInRoleAsync(user, role));
                }

                if(isInRole)
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

            return medWorkers;
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
