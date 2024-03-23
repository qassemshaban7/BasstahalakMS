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
            var users = await _context.ApplicationUsers.Where(u => u.Type != null).ToListAsync();

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
                    Type = model.Type,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, StaticDetails.Printing);
                    await _context.SaveChangesAsync();

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

            var editPrintingVM = new EditPrintingVM
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Type = user.Type
            };

            return View(editPrintingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,UserName,Password,Address,PhoneNumber,Type")] EditPrintingVM editPrintingVM)
        {
            if (id != editPrintingVM.Id)
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

                user.FullName = editPrintingVM.FullName;
                user.UserName = editPrintingVM.UserName;
                user.Address = editPrintingVM.Address;
                user.PhoneNumber = editPrintingVM.PhoneNumber;
                user.Type = editPrintingVM.Type;

                _context.Update(user);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(editPrintingVM);
        }

        public async Task<IActionResult> Delete(string id)
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

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // دفع فاتورة
        public async Task<IActionResult> CreatePayment(string Id)
        {
            var user = await _context.ApplicationUsers.FindAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            var payment = new Payment
            {
                UserId = Id,
                User = user
            };

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePayment(Payment payment, IFormFile file, string UserId) 
        {
            try
            {
                payment.UserId = UserId;

                if (file != null && file.Length > 0)
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".gif")
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine("wwwroot", "Payment", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    payment.PhotoPath = fileName;
                }

                payment.PaymentTime = DateTime.Now;
                _context.Payments.Add(payment);
                var user = _context.ApplicationUsers.Find(UserId);
                if (user.TotalMoney == null) user.TotalMoney = 0;
                user.TotalMoney -= payment.Money;
                await _context.SaveChangesAsync();
                return RedirectToAction( "Index", "Payment");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطا اعد المحاولة");
                return View(payment);
            }
        }
    }
}