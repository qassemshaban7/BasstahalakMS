using BasstahalakMS.Data;
using BasstahalakMS.Utility;
using BasstahalakMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password(string oldPassword, string newPassword)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            int x = 0;
            if (oldPassword == null && newPassword == null)
            {
                {
                    x = 2;
                    return View("ChangePassword", new ChangePasswordViewModel { X = x });
                }
            }

            var passwordVerificationResult = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!passwordVerificationResult)
            {
                x = 1;
                return View("ChangePassword",  new  ChangePasswordViewModel { X = x });
            }

            // P@ssw0rd
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {
                return View("ChangePassword", new ChangePasswordViewModel());
            }
        }
    }
}
