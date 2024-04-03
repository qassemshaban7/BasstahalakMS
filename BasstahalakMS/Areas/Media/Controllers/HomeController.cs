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

namespace BasstahalakMS.Areas.Media.Controllers
{
    [Authorize(Roles = StaticDetails.Media)]
    [Area(nameof(Media))]
    [Route(nameof(Media) + "/[controller]/[action]")]
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
            ViewBag.ThisUser = user;
            var acceptedNotes = _context.BfileNotes
                .Where(x => x.ReciveUserId == userId && (x.status == 7))
                .Include(c => c.BFile)
                .ThenInclude(c => c.Book)
                .Include(c => c.BFile)
                .ThenInclude(c => c.User)
                .AsQueryable();

            var AcceptedBFiles = await acceptedNotes.GroupBy(x => x.BFile.Id)
                .Select(group => group.First())
                .CountAsync();

            ViewBag.accCounter = AcceptedBFiles;

            var pdfs = await _context.PdfFiles.Where(x => x.UserId == userId).Include(c => c.User).CountAsync();
            ViewBag.pdf = pdfs;

            if (user.IsAdmin == 1)
            {

                SuperAdminHomeVM homeVM = new SuperAdminHomeVM
                {
                    MediaTeam = await (from x in _context.ApplicationUsers
                                       join userRole in _context.UserRoles
                                       on x.Id equals userRole.UserId
                                       join role in _context.Roles
                                       on userRole.RoleId equals role.Id
                                       where role.Name == StaticDetails.Media
                                       select x)
                                      .ToListAsync(),
                };
                return View(homeVM);
            }
            return View();
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
            if (newPassword == null)
            {
                x = 2;
                return View("ChangePassword", new ChangePasswordViewModel { X = x });
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
