using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using BasstahalakMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BasstahalakMS.Areas.Media.Controllers
{
    [Authorize(Roles = StaticDetails.Media)]
    [Area(nameof(Media))]
    [Route(nameof(Media) + "/[controller]/[action]")]
    public class MediaUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public MediaUsersController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment
            , UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.ApplicationUsers.FindAsync(userId);
            if (user.IsAdmin == 1)
            {
                if (HttpContext.Session.GetString("created") != null)
                {
                    ViewBag.created = true;
                    HttpContext.Session.Remove("created");
                }
                if (HttpContext.Session.GetString("updated") != null)
                {
                    ViewBag.updated = true;
                    HttpContext.Session.Remove("updated");
                }
                if (HttpContext.Session.GetString("deleted") != null)
                {
                    ViewBag.deleted = true;
                    HttpContext.Session.Remove("deleted");
                }
                // Get a list of users in the role
                //var usersWithPermission = _userManager.GetUsersInRoleAsync(StaticDetails.Prepare).Result.ToList();
                var users = await (from x in _context.ApplicationUsers
                                   join userRole in _context.UserRoles
                                   on x.Id equals userRole.UserId
                                   join role in _context.Roles
                                   on userRole.RoleId equals role.Id
                                   where role.Name == StaticDetails.Media
                                   select x)
                                   .ToListAsync();
                return View(users);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public async Task<IActionResult> Create(string? returnUrl = null)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.ApplicationUsers.FindAsync(userId);
            if (user.IsAdmin == 1)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserVM model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    FullName = model.FullName,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    IsAdmin = 0,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, StaticDetails.Media);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("created", "true");
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CurrentUser = await _context.ApplicationUsers.FindAsync(userId);
            if (CurrentUser.IsAdmin == 1)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.ApplicationUsers.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var editUserVM = new EditUserVM
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    IsAdmin = user.IsAdmin
                };

                return View(editUserVM);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,UserName,Password,PhoneNumber,IsAdmin")] EditUserVM editUserVM)
        {
            if (id != editUserVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _context.ApplicationUsers.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FullName = editUserVM.FullName;
                user.UserName = editUserVM.UserName;
                user.PhoneNumber = editUserVM.PhoneNumber;

                _context.Update(user);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("updated", "true");
                return RedirectToAction(nameof(Index));
            }
            return View(editUserVM);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers.FindAsync(id);
            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("deleted", "true");
            return RedirectToAction(nameof(Index));
        }


        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
