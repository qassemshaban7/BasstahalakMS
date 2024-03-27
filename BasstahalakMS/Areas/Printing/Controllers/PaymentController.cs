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
using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;

namespace BasstahalakMS.Areas.Printing.Controllers
{
    [Authorize(Roles = StaticDetails.Printing)]
    [Area("Printing")]
    [Route(nameof(Printing) + "/[controller]/[action]")]
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
            if (HttpContext.Session.GetString("created") != null)
            {
                ViewBag.created = true;
                HttpContext.Session.Remove("created");
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.ApplicationUsers.FindAsync(userId);
            ViewBag.ThisUser = user;

            var payments = await _context.Payments.Where(c => c.UserId == userId).Include(c => c.User).ToListAsync();
            return View(payments);
        }
    }
}
