using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BasstahalakMS.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticDetails.SuperAdmin)]
    [Area(nameof(SuperAdmin))]
    [Route(nameof(SuperAdmin) + "/[controller]/[action]")]
    public class PrintTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrintTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var printTypes = await _context.PrintTypes.ToListAsync();
            return View(printTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrintType printType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(printType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(printType);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var printType = await _context.PrintTypes.FindAsync(id);
            if (printType == null)
            {
                return NotFound();
            }

            return View(printType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PrintType printType)
        {
            if (id != printType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(printType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrintTypeExists(printType.Id))
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
            return View(printType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var printType = await _context.PrintTypes.FindAsync(id);
            if (printType == null)
            {
                return NotFound();
            }

            return View(printType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var printType = await _context.PrintTypes.FindAsync(id);
            _context.PrintTypes.Remove(printType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrintTypeExists(int id)
        {
            return _context.PrintTypes.Any(e => e.Id == id);
        }
    }
}