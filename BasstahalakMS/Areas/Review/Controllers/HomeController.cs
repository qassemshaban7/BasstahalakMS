using BasstahalakMS.Data;
using BasstahalakMS.Utility;
using BasstahalakMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace BasstahalakMS.Areas.Review.Controllers
{
    [Authorize(Roles = StaticDetails.Review)]
    [Area(nameof(Review))]
    [Route(nameof(Review) + "/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.ApplicationUsers.FindAsync(userId);
            var bFileNotes = await _context.BfileNotes.Where(x => x.UserId == userId && x.status == 3 || x.status ==4).ToListAsync();
            var bFiles = await _context.BFiles.Include(x => x.Book).Include(x => x.User).Where(x => x.status == 3 || x.status == 4).ToListAsync();
            if(bFileNotes.Count() == 0)
            {
                bFiles.RemoveAll(x=>x.UserId != null);
            }
            else
            {
                foreach (var item in bFiles.ToList())
                {
                    foreach (var item1 in bFileNotes)
                    {
                        if (item.Id != item1.BfileId)
                            bFiles.Remove(item);
                    }
                }
            }
            
            SuperAdminHomeVM homeVM = new SuperAdminHomeVM
            {
                BFiles = bFiles
            };
            if (user.IsAdmin == 1)
            {
                ViewBag.IsAdmin = 1;
                var users = await (from x in _context.ApplicationUsers
                                   join userRole in _context.UserRoles
                                   on x.Id equals userRole.UserId
                                   join role in _context.Roles
                                   on userRole.RoleId equals role.Id
                                   where role.Name == StaticDetails.Review
                                   select x)
                                  .ToListAsync();
                ViewBag.usersCount = users.Count();
               
            }
           
            return View(homeVM);
        }
    }
}
