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
                ReviewTeam = await (from x in _context.ApplicationUsers
                                    join userRole in _context.UserRoles
                                    on x.Id equals userRole.UserId
                                    join role in _context.Roles
                                    on userRole.RoleId equals role.Id
                                    where role.Name == StaticDetails.Review
                                    select x)
                                  .ToListAsync(),
                PrepareTeam = await (from x in _context.ApplicationUsers
                                     join userRole in _context.UserRoles
                                     on x.Id equals userRole.UserId
                                     join role in _context.Roles
                                     on userRole.RoleId equals role.Id
                                     where role.Name == StaticDetails.Prepare
                                     select x)
                                  .ToListAsync(),
                BFiles = await _context.BFiles.Where(x=>x.status!=0).ToListAsync(),
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
