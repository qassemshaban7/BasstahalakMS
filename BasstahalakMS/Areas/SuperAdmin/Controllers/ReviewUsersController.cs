using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using BasstahalakMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BasstahalakMS.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticDetails.SuperAdmin)]
    [Area(nameof(SuperAdmin))]
    [Route(nameof(SuperAdmin) + "/[controller]/[action]")]
    public class ReviewUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ReviewUsersController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment
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
                if (HttpContext.Session.GetString("SupervisorsPermissionsDone") != null)
                {
                    ViewBag.SupervisorsPermissionsDone = true;
                    HttpContext.Session.Remove("SupervisorsPermissionsDone");
                }
            // Get a list of users in the role
            //var usersWithPermission = _userManager.GetUsersInRoleAsync(StaticDetails.Prepare).Result.ToList();
            var users = await (from x in _context.ApplicationUsers
                                   join userRole in _context.UserRoles
                                   on x.Id equals userRole.UserId
                                   join role in _context.Roles
                                   on userRole.RoleId equals role.Id
                                   where role.Name == StaticDetails.Review
                                   select x)
                                   .ToListAsync();
                return View(users);

           

        }
        public async Task<IActionResult> Create(string? returnUrl = null)
        {
            
                ViewData["ReturnUrl"] = returnUrl;
                return View();
          

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
                    IsAdmin = model.IsAdmin,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, StaticDetails.Review);
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
                user.IsAdmin = Convert.ToInt32(editUserVM.IsAdmin);

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
        public async Task<IActionResult> SupervisorsPermissions()
        {
            var ReviewAdmins = await (from x in _context.ApplicationUsers
                                      join userRole in _context.UserRoles
                                      on x.Id equals userRole.UserId
                                      join role in _context.Roles
                                      on userRole.RoleId equals role.Id
                                      where role.Name == StaticDetails.Review
                                      where x.IsAdmin == 1
                                      select x)
                                 .ToListAsync();
            
            ViewBag.ReviewAdmins = ReviewAdmins;
            return View(await _context.ReviewPermissions.Where(x=>x.IsAdmin == 1).ToListAsync());


        }
        public async Task<IActionResult> UsersPermissions()
        {

            var ReviewUsers = await (from x in _context.ApplicationUsers
                                      join userRole in _context.UserRoles
                                      on x.Id equals userRole.UserId
                                      join role in _context.Roles
                                      on userRole.RoleId equals role.Id
                                      where role.Name == StaticDetails.Review
                                      where x.IsAdmin == 0
                                      select x)
                                  .ToListAsync();

            ViewBag.ReviewUsers = ReviewUsers;
            return View(await _context.ReviewPermissions.Where(x => x.IsAdmin == 0).ToListAsync());


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPermissionsToSupervisors(int[] Permissions, string[] ReviewSupervisors,int allSupervisors)
        {
            try
            {
                if(allSupervisors == 1)
                {
                    var ReviewAdmins = await (from x in _context.ApplicationUsers
                                              join userRole in _context.UserRoles
                                              on x.Id equals userRole.UserId
                                              join role in _context.Roles
                                              on userRole.RoleId equals role.Id
                                              where role.Name == StaticDetails.Review
                                              where x.IsAdmin == 1
                                              select x)
                                .ToListAsync();
                    foreach (var item in ReviewAdmins)
                    {
                        var existingPermissions = await _context.UserReviewPermissions.Where(x => x.UserId == item.Id).ToListAsync();
                        foreach (var item1 in existingPermissions)
                        {
                            _context.UserReviewPermissions.Remove(item1);
                            await _context.SaveChangesAsync();
                        }
                        for (int j = 0; j < Permissions.Count(); j++)
                        {
                            UserReviewPermission userReviewPermission = new UserReviewPermission
                            {
                                PermissionId = Convert.ToInt32(Permissions.GetValue(j)),
                                UserId = item.Id,

                            };
                            _context.UserReviewPermissions.Add(userReviewPermission);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ReviewSupervisors.Count(); i++)
                    {
                        var existingPermissions = await _context.UserReviewPermissions.Where(x => x.UserId == ReviewSupervisors.GetValue(i).ToString()).ToListAsync();
                        foreach (var item in existingPermissions)
                        {
                            _context.UserReviewPermissions.Remove(item);
                            await _context.SaveChangesAsync();
                        }
                        for (int j = 0; j < Permissions.Count(); j++)
                        {
                            UserReviewPermission userReviewPermission = new UserReviewPermission
                            {
                                PermissionId = Convert.ToInt32(Permissions.GetValue(j)),
                                UserId = ReviewSupervisors.GetValue(i).ToString(),

                            };
                            _context.UserReviewPermissions.Add(userReviewPermission);
                            await _context.SaveChangesAsync();
                        }
                    }

                }
                HttpContext.Session.SetString("SupervisorsPermissionsDone", "true");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPermissionsToUsers(int[] Permissions, string[] ReviewUsers, int allUsers)
        {
            try
            {
                if (allUsers == 1)
                {
                    var reviewers = await (from x in _context.ApplicationUsers
                                              join userRole in _context.UserRoles
                                              on x.Id equals userRole.UserId
                                              join role in _context.Roles
                                              on userRole.RoleId equals role.Id
                                              where role.Name == StaticDetails.Review
                                              where x.IsAdmin == 0
                                              select x)
                                .ToListAsync();
                    foreach (var item in reviewers)
                    {
                        var existingPermissions = await _context.UserReviewPermissions.Where(x => x.UserId == item.Id).ToListAsync();
                        foreach (var item1 in existingPermissions)
                        {
                            _context.UserReviewPermissions.Remove(item1);
                            await _context.SaveChangesAsync();
                        }
                        for (int j = 0; j < Permissions.Count(); j++)
                        {
                            UserReviewPermission userReviewPermission = new UserReviewPermission
                            {
                                PermissionId = Convert.ToInt32(Permissions.GetValue(j)),
                                UserId = item.Id,

                            };
                            _context.UserReviewPermissions.Add(userReviewPermission);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ReviewUsers.Count(); i++)
                    {
                        var existingPermissions = await _context.UserReviewPermissions.Where(x => x.UserId == ReviewUsers.GetValue(i).ToString()).ToListAsync();
                        foreach (var item in existingPermissions)
                        {
                            _context.UserReviewPermissions.Remove(item);
                            await _context.SaveChangesAsync();
                        }
                        for (int j = 0; j < Permissions.Count(); j++)
                        {
                            UserReviewPermission userReviewPermission = new UserReviewPermission
                            {
                                PermissionId = Convert.ToInt32(Permissions.GetValue(j)),
                                UserId = ReviewUsers.GetValue(i).ToString(),

                            };
                            _context.UserReviewPermissions.Add(userReviewPermission);
                            await _context.SaveChangesAsync();
                        }
                    }

                }
                HttpContext.Session.SetString("SupervisorsPermissionsDone", "true");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return RedirectToAction(nameof(Index));
            }

        }

        public async Task<IActionResult> ShowOneUserPermissions(string Id)
        {
            var user = await _context.ApplicationUsers.FindAsync(Id);
            var allPermissions = new List<ReviewPermission>();
            int allPermissionsCount = 0;
            if (user.IsAdmin == 0)
            {
                 allPermissions = await _context.ReviewPermissions.Where(x => x.IsAdmin == 0).ToListAsync();
                 allPermissionsCount = allPermissions.Count();
            }
            else
            {
                 allPermissions = await _context.ReviewPermissions.Where(x => x.IsAdmin == 1).ToListAsync();
                 allPermissionsCount = allPermissions.Count();

            }
            var currentUserPermissions = await _context.UserReviewPermissions
                    .Include(x => x.ReviewPermission).Where(x => x.UserId == Id).ToListAsync();
            foreach (var item in allPermissions.ToList())
            {
                foreach (var item1 in currentUserPermissions)
                {
                    if (item.Id == item1.PermissionId)
                        allPermissions.Remove(item);
                }
            }
            ViewBag.User = user;
            ViewBag.currentUserPermissions = currentUserPermissions;
            ViewBag.currentUserPermissionsCount = currentUserPermissions.Count();
            ViewBag.allPermissionsCount = allPermissionsCount;
            return View(allPermissions);

        }
    }
}
