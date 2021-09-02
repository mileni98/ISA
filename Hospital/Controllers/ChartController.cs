using Hospital.Data;
using Hospital.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NHospital.Controllers
{
    public class ChartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ChartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var reservations = _context.Reservation.ToList();
            var dataList = new List<ChartDTO>();

            var thisM = _context.Reservation
                            .Where(x => x.StartTime > DateTime.Now.AddMonths(-1)
                                        && x.StartTime < DateTime.Now.AddMonths(1))
                            .ToList();

            for (var i = 0; i < 8; i++)
            {
                ChartDTO chartDTO = new ChartDTO();

                chartDTO.name = DateTime.Now.AddMonths(i - 8).ToString("MMMM");
                chartDTO.score = _context.Reservation
                                        .Where(x => x.StartTime >= DateTime.Now.AddMonths(i - 8)
                                        && x.StartTime <= DateTime.Now.AddMonths(i - 8 + 1)).ToList().Count;
                dataList.Add(chartDTO);
            }

            var month = DateTime.Now.ToString("MMMM");
            ViewData["data"] = JsonSerializer.Serialize(dataList);
            ViewData["max"] = dataList.Select(x => x.score).Max();
            return View();
        }
    }
}
