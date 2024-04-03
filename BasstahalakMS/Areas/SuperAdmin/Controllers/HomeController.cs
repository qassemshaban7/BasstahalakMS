using BasstahalakMS.Data;
using BasstahalakMS.Utility;
using BasstahalakMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace BasstahalakMS.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticDetails.SuperAdmin)]
    [Area(nameof(SuperAdmin))]
    [Route(nameof(SuperAdmin) + "/[controller]/[action]")]
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
            SuperAdminHomeVM homeVM = new SuperAdminHomeVM
            {
                Users = await _context.ApplicationUsers.Where(x => x.Type == 1 || x.Type == 2).ToListAsync(),
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
                MediaTeam = await (from x in _context.ApplicationUsers
                                   join userRole in _context.UserRoles
                                   on x.Id equals userRole.UserId
                                   join role in _context.Roles
                                   on userRole.RoleId equals role.Id
                                   where role.Name == StaticDetails.Media
                                   select x)
                                  .ToListAsync(),

                BFiles = await _context.BFiles.Where(x => x.status != 0).ToListAsync(),
                Books = await _context.Books.ToListAsync(),
                Branches = await _context.Branches.ToListAsync(),
                PrintTypes = await _context.PrintTypes.ToListAsync(),
                Libraries = await _context.Libraries.ToListAsync(),
                Payments = await _context.Payments.ToListAsync()
            };

            var AcceptedBFilesCount = await _context.BFiles
                .Where(x => x.status == 10 || x.status == 7)
                .CountAsync();

            ViewBag.accCounter = AcceptedBFilesCount;

            var RejectedBFiles = await _context.BFiles
                .Where(x => x.status == 5 || x.status == 2 || x.status == 3 || x.status == 4)
                .CountAsync();

            ViewBag.Rejected = RejectedBFiles; 

            return View(homeVM);


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
                return View("ChangePassword", new ChangePasswordViewModel { X = x });
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
