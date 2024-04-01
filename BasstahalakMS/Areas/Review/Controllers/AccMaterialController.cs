using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BasstahalakMS.Areas.Review.Controllers
{
    [Authorize(Roles = StaticDetails.Review)]
    [Area(nameof(Review))]
    [Route(nameof(Review) + "/[controller]/[action]")]
    public class AccMaterialController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AccMaterialController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.ApplicationUsers.FindAsync(userId);

                var bFileNotes = await _context.BfileNotes
                    .Where(x => x.ReciveUserId == userId && (x.status ==10))
                    .Include(c => c.BFile)
                    .ThenInclude(c => c.Book)
                    .Include(c => c.BFile)
                    .ThenInclude(c => c.User)
                    .ToListAsync();

                var distinctBFiles = bFileNotes.GroupBy(x => x.BFile.Id)
                                                .Select(group => group.First())
                                                .ToList();
                
                ViewBag.ThisUser = user;
                return View(distinctBFiles);
        }

        public async Task<IActionResult> AcceptMaterial(int id)
        {
            var existingFile = await _context.BFiles.FindAsync(id);
            if (existingFile == null)
            {
                return NotFound();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            var books = _context.Books.ToList();
            ViewBag.Books = books;

            var branches = _context.Branches.ToList();
            var currentBranches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();

            foreach (var item in branches.ToList())
            {
                foreach (var item1 in currentBranches)
                {
                    if (item.Id == item1.BranchId)
                        branches.Remove(item);
                }
            }
            ViewBag.Branches = branches;

            ViewBag.currentBranches = currentBranches;
            ViewBag.currentBranchesCount = currentBranches.Count();

            if (existingFile.status == -1)
            {
                var bfileNotess = await _context.BfileNotes
                                .Where(p => p.BfileId == existingFile.Id && p.status == 10)
                                .Include(x => x.BFile)
                                .Include(x => x.User)
                                .ToListAsync();

                ViewBag.bfileNote = bfileNotess;
                return View(existingFile);
            }

            return View(existingFile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptMaterial(BFile bFile)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var existingFile = await _context.BFiles.FindAsync(bFile.Id);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
