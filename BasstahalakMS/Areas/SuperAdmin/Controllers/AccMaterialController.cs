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
                .Where(x => x.status == 10 || x.status == 7)
                .Include(c => c.User)
                .Include(c => c.BFile)
                .ThenInclude(c => c.Book)
                .Include(c => c.BFile)
                .ThenInclude(c => c.User)
                .ToListAsync();

            var distinctBFiles = bFileNotes.OrderByDescending(c => c.CreationDate).GroupBy(x => x.BFile.Id)
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
                                .Where(p => p.BfileId == existingFile.Id && p.status == 10 | p.status == 7)
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


        public async Task<IActionResult> ShowFile(int id)
        {
            var existingFile = await _context.BFiles.Include(c => c.Book).Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
            if (existingFile == null)
            {
                return NotFound();
            }
            var MediaAdmins = await (from x in _context.ApplicationUsers
                                     join userRole in _context.UserRoles
                                     on x.Id equals userRole.UserId
                                     join role in _context.Roles
                                     on userRole.RoleId equals role.Id
                                     where role.Name == StaticDetails.Media
                                     where x.IsAdmin == 1
                                     select x)
                                 .ToListAsync();
            var MediaUsers = await (from x in _context.ApplicationUsers
                                    join userRole in _context.UserRoles
                                    on x.Id equals userRole.UserId
                                    join role in _context.Roles
                                    on userRole.RoleId equals role.Id
                                    where role.Name == StaticDetails.Media
                                    where x.IsAdmin != 1
                                    select x)
                                 .ToListAsync();

            ViewBag.ReviewAdmins = MediaAdmins;
            ViewBag.ReviewUsers = MediaUsers;

            var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
            ViewBag.branches = branches;
            return View(existingFile);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForReview(string fileContent, string Notes, int Prepare, int BfileId)
        {
            try
            {

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (Prepare == -20)
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = fileContent,
                        Notes = Notes,
                        SendUserId = userId,
                        status = 2 // Back to Prepare
                    };
                    _context.BfileNotes.Add(bfileNote);
                    var bfile = await _context.BFiles.FindAsync(BfileId);
                    bfile.status = 2;
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("Sent", "true");
                    return RedirectToAction(nameof(Index));
                }

                else if (Prepare == 2)
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = fileContent,
                        Notes = Notes,
                        SendUserId = userId,
                        status = 7 // Sent to Review
                    };
                    _context.BfileNotes.Add(bfileNote);
                    var bfile = await _context.BFiles.FindAsync(BfileId);
                    bfile.status = 7;
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("Sent", "true");
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForReviewTeam(string ReviewSupervisor, int Reviewers, string[] ReviewUsers, int BfileId)
        {
            try
            {

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var bfile = await _context.BFiles.FindAsync(BfileId);

                if (bfile.status == 10)
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = bfile.fileContent,
                        Notes = "",
                        ReciveUserId = ReviewSupervisor,
                        SendUserId = userId,
                        status = 7 // Sent to Media
                    };
                    _context.BfileNotes.Add(bfileNote);
                    bfile.status = 7;
                    bfile.TeamStatus = 1;
                    if (Reviewers == 1)
                    {
                        var reviewers = await (from x in _context.ApplicationUsers
                                               join userRole in _context.UserRoles
                                               on x.Id equals userRole.UserId
                                               join role in _context.Roles
                                               on userRole.RoleId equals role.Id
                                               where role.Name == StaticDetails.Review
                                               where x.IsAdmin != 1
                                               select x)
                                      .ToListAsync();
                        foreach (var reviewer in reviewers)
                        {
                            BfileNote bfileNote1 = new BfileNote
                            {
                                BfileId = BfileId,
                                CurrentFileContent = bfile.fileContent,
                                Notes = "",
                                ReciveUserId = reviewer.Id,
                                SendUserId = userId,
                                status = 7 // Sent to Team Media
                            };
                            _context.BfileNotes.Add(bfileNote1);
                            bfile.status = 7;
                            bfile.TeamStatus = 0;

                        }
                    }
                    else if (Reviewers == 2)
                    {
                        for (int i = 0; i < ReviewUsers.Count(); i++)
                        {
                            var reviewer = await _context.ApplicationUsers.FindAsync(ReviewUsers.GetValue(i));
                            if (reviewer.IsAdmin == 1)
                            {
                                BfileNote bfileNote2 = new BfileNote
                                {
                                    BfileId = BfileId,
                                    CurrentFileContent = bfile.fileContent,
                                    Notes = "",
                                    ReciveUserId = reviewer.Id,
                                    SendUserId = userId,
                                    status = 7 // Sent to Supervisor of team Media
                                };
                                _context.BfileNotes.Add(bfileNote2);
                                bfile.status = 7;
                                bfile.TeamStatus = 1;
                            }
                            else
                            {
                                BfileNote bfileNote2 = new BfileNote
                                {
                                    BfileId = BfileId,
                                    CurrentFileContent = bfile.fileContent,
                                    Notes = "",
                                    ReciveUserId = reviewer.Id,
                                    SendUserId = userId,
                                    status = 7 // Sent to Team Media
                                };
                                _context.BfileNotes.Add(bfileNote2);
                                bfile.status = 7;
                                bfile.TeamStatus = 0;
                            }

                        }
                    }

                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("Sent", "true");
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return RedirectToAction(nameof(Index));
            }

        }


    }
}
