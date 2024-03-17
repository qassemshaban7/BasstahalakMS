using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using BasstahalakMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BasstahalakMS.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticDetails.SuperAdmin)]
    [Area(nameof(SuperAdmin))]
    [Route(nameof(SuperAdmin) + "/[controller]/[action]")]
    public class PrintingController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public PrintingController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment
            , UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("created") != null)
            {
                ViewBag.created = true;
                HttpContext.Session.Remove("created");
            }
            // Get a list of users in the role
            var usersWithPermission = _userManager.GetUsersInRoleAsync(StaticDetails.Printing).Result;

            // Then get a list of the ids of these users
            var idsWithPermission = usersWithPermission.Select(u => u.Id);

            // Now get the users in our database with the same ids
            var users = await _context.ApplicationUsers.Where(u => idsWithPermission.Contains(u.Id)).ToListAsync();

            return View(users);
        }
        public IActionResult Create(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePrintingVM model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    FullName = model.FullName,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Type = model.Type
                };

                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, StaticDetails.Printing);

                    HttpContext.Session.SetString("created", "true");
                    return RedirectToAction("Index", "Printing", new { area = "SuperAdmin" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

    }

}
