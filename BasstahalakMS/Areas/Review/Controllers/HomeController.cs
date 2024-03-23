using BasstahalakMS.Data;
using BasstahalakMS.Utility;
using BasstahalakMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BasstahalakMS.Areas.Review.Controllers
{
    [Authorize(Roles = StaticDetails.Review)]
    [Area(nameof(Review))]
    [Route(nameof(Review) + "/[controller]")]
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
                BFiles = await _context.BFiles.Where(f=>f.status == 3).ToListAsync()
            };
            return View(homeVM);
        }
    }
}
