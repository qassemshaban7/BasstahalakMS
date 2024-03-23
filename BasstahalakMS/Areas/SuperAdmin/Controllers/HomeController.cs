using BasstahalakMS.Data;
using BasstahalakMS.Utility;
using BasstahalakMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BasstahalakMS.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticDetails.SuperAdmin)]
    [Area(nameof(SuperAdmin))]
    [Route(nameof(SuperAdmin) + "/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            SuperAdminHomeVM homeVM = new SuperAdminHomeVM
            {
                Users = await _context.ApplicationUsers.Where(x=>x.Type == 1 || x.Type == 2).ToListAsync(),
                BFiles = await _context.BFiles.ToListAsync(),
                Books = await _context.Books.ToListAsync(),
                Branches = await _context.Branches.ToListAsync(),
                PrintTypes = await _context.PrintTypes.ToListAsync(),
                Libraries = await _context.Libraries.ToListAsync(),
                Payments = await _context.Payments.ToListAsync()
            };
            return View(homeVM);
        }
    }
}
