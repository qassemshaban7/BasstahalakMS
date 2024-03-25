using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
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
    public class OrderPrintingController : Controller
    {

        private readonly ApplicationDbContext _context;

        public OrderPrintingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("created") != null)
            {
                ViewBag.created = true;
                HttpContext.Session.Remove("created");
            }
            var libraries = await _context.Libraries
                .Include(l => l.PrintType)
                .Include(l => l.User)
                .ToListAsync();

            return View(libraries);
        }

        public async Task<IActionResult> Request(int id)
        {
            var library = await _context.Libraries
                .Include(l => l.User)
                .Include(l => l.PrintType)
                .FirstOrDefaultAsync(x => x.LibraryId == id);

            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Request(Library library)
        {
            if (library.Status != 1)
            {
                var order = await _context.Libraries.FindAsync(library.LibraryId);
                if (order != null)
                {
                    order.Status = library.Status;
                    order.Notes = library.Notes;

                    _context.Update(order);
                    await _context.SaveChangesAsync();

                    if (order.Status == 3 && order.UserId != null)
                    {
                        var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == order.UserId);
                        if (user != null)
                        {
                            if (user.TotalMoney == null) user.TotalMoney = 0;

                            user.TotalMoney += order.Total;
                            _context.Update(user);
                            await _context.SaveChangesAsync();
                        }
                    }
                    HttpContext.Session.SetString("created", "true");
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
