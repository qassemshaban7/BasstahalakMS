using BasstahalakMS.Data;
using BasstahalakMS.Utility;
using BasstahalakMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BasstahalakMS.Areas.Printing.Controllers  
{
    [Authorize(Roles = StaticDetails.Printing)]
    [Area(nameof(Printing))]
    [Route(nameof(Printing) + "/[controller]")] 
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public async Task<IActionResult> Index()
        {
            SuperAdminHomeVM homeVM = new SuperAdminHomeVM
            {
                Libraries = await _context.Libraries.ToListAsync()
            };

            return View(homeVM);
        }
    }
}
