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

namespace BasstahalakMS.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticDetails.Admin)]
    [Area("Admin")]
    [Route(nameof(Admin) + "/[controller]/[action]")]
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
                    var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                    var filePath = System.IO.Path.Combine("wwwroot", "Payment", fileName);
                    using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    payment.PhotoPath = fileName;
                }

                payment.PaymentTime = DateTime.Now;
                _context.Payments.Add(payment);
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
