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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: 
        [Authorize(Roles = "Admin,Pharmacist,Doctor")]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();

            var usersDTO = new List<UserDTO>();

            foreach(var user in users)
            {
                string role = await GetUserRole(user);
                usersDTO.Add(new UserDTO(user.Email, role));
            }
            return View(usersDTO);
        }

        public async Task<string> GetUserRole(IdentityUser user)
        {
            return (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        }
    }
}
