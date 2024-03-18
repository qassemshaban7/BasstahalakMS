using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasstahalakMS.Areas.Printing.Controllers
{
    [Authorize(Roles = StaticDetails.Printing)]
    [Area(nameof(Printing))]
    [Route(nameof(Printing) + "/[controller]/[action]")]
    public class LibraryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibraryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var libraries = await _context.Libraries
                .Include(l => l.PrintType)
                .Where(l => l.UserId == userId)
                .ToListAsync();
            return View(libraries);
        }

        public IActionResult Create()
        {
            ViewBag.PrintTypeId = new SelectList(_context.PrintTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibraryId,Color,Count,PriceOfUnit,PrintTypeId")] Library library)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                library.UserId = userId;

                if (library.Count <= 0 || library.PriceOfUnit <= 0)
                {
                    ModelState.AddModelError(string.Empty, "الكمية والسعر يجب أن يكونا أكبر من صفر.");
                    ViewBag.PrintTypeId = new SelectList(_context.PrintTypes, "Id", "Name", library.PrintTypeId);
                    return View(library);
                }

                library.Total = library.Count * library.PriceOfUnit;
                _context.Add(library);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ أثناء معالجة الطلب. يرجى المحاولة مرة أخرى.");
                ViewBag.PrintTypeId = new SelectList(_context.PrintTypes, "Id", "Name", library.PrintTypeId);
                return View(library);
            }
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _context.Libraries.FindAsync(id);
            if (library == null)
            {
                return NotFound();
            }
            ViewBag.PrintTypeId = new SelectList(_context.PrintTypes, "Id", "Name", library.PrintTypeId);
            return View(library);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibraryId,Color,Count,PriceOfUnit,PrintTypeId")] Library library)
        {
            if (id != library.LibraryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    library.UserId = userId;
                    library.Total = library.Count * library.PriceOfUnit;
                    _context.Update(library);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryExists(library.LibraryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.PrintTypeId = new SelectList(_context.PrintTypes, "Id", "Name", library.PrintTypeId);
            return View(library);
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var library = await _context.Libraries
        //        .Include(l => l.PrintType)
        //        .FirstOrDefaultAsync(m => m.LibraryId == id);
        //    if (library == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(library);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var library = await _context.Libraries.FindAsync(id);
        //    _context.Libraries.Remove(library);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool LibraryExists(int id)
        {
            return _context.Libraries.Any(e => e.LibraryId == id);
        }
    }
}
