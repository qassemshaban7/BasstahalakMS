using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BasstahalakMS.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticDetails.SuperAdmin)]
    [Area("SuperAdmin")]
    [Route(nameof(SuperAdmin) + "/[controller]/[action]")]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Sent") != null)
            {
                ViewBag.Sent = true;
                HttpContext.Session.Remove("Sent");
            }
            var payments = await _context.Payments.Include(c => c.User).ToListAsync();
            return View(payments);
        }

        [HttpGet]
        public IActionResult GetTotalMoney(string userId)
        {
            var user = _context.ApplicationUsers.Find(userId);
            var totalMoney = user.TotalMoney;

            return Json(totalMoney);
        }
        public async Task<IActionResult> Create()
        {
            var users = await _context.ApplicationUsers.Where(u => u.Type != null).ToListAsync();
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment, IFormFile file)
        {
            try
            {
                if (string.IsNullOrEmpty(payment.UserId))
                {
                    ModelState.AddModelError(string.Empty, "اختر اسم المطبعة.");
                    return View(payment);
                }

                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine("wwwroot", "Payment", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    payment.PhotoPath = fileName;
                }

                payment.PaymentTime = DateTime.Now;
                _context.Payments.Add(payment);
                var user = _context.ApplicationUsers.Find(payment.UserId);
                if (user.TotalMoney == null) user.TotalMoney = 0;
                user.TotalMoney -= payment.Money;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطا اعد المحاولة");
                return View(payment);
            }
        }
    }
}
