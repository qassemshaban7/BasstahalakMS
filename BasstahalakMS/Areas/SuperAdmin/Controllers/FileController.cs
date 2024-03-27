using BasstahalakMS.Data;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Syncfusion.EJ2.Navigations;
using System.Security.Claims;
using BasstahalakMS.Models;

namespace BasstahalakMS.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticDetails.SuperAdmin)]
    [Area(nameof(SuperAdmin))]
    [Route(nameof(SuperAdmin) + "/[controller]/[action]")]
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            //if (HttpContext.Session.GetString("created") != null)
            //{
            //    ViewBag.created = true;
            //    HttpContext.Session.Remove("created");
            //}
            if (HttpContext.Session.GetString("Sent") != null)
            {
                ViewBag.Sent = true;
                HttpContext.Session.Remove("Sent");
            }
            var files = await _context.BFiles.Include(x => x.Book).Include(c=>c.User).Where(x=>x.status !=0).ToListAsync();
            return View(files);
        }
		public async Task<IActionResult> ShowFile(int id)
		{
			var existingFile = await _context.BFiles.Include(c=>c.Book).Include(c=>c.User).FirstOrDefaultAsync(x=>x.Id == id);
			if (existingFile == null)
			{
				return NotFound();
			}
            var ReviewAdmins = await (from x in _context.ApplicationUsers
                               join userRole in _context.UserRoles
                               on x.Id equals userRole.UserId
                               join role in _context.Roles
                               on userRole.RoleId equals role.Id
                               where role.Name == StaticDetails.Review
                               where x.IsAdmin == 1
                               select x)
                                 .ToListAsync();
            var ReviewUsers = await (from x in _context.ApplicationUsers
                                      join userRole in _context.UserRoles
                                      on x.Id equals userRole.UserId
                                      join role in _context.Roles
                                      on userRole.RoleId equals role.Id
                                      where role.Name == StaticDetails.Review
                                      where x.IsAdmin != 1
                                      select x)
                                 .ToListAsync();
            ViewBag.ReviewAdmins = ReviewAdmins;
            ViewBag.ReviewUsers = ReviewUsers;

            var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
            ViewBag.branches = branches;
            return View(existingFile);

		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForReview(string fileContent , string Notes, int Prepare, int BfileId)
        {
            try
            {
                
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if(Prepare == 1)
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = fileContent,
                        Notes = Notes,
                        UserId = userId,
                        status = 2 // Back to Prepare
                    };
                    _context.BfileNotes.Add(bfileNote);
                    var bfile = await _context.BFiles.FindAsync(BfileId);
                    bfile.status = 2;
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("Sent", "true");
                    return RedirectToAction(nameof(Index));
                }
               
                else
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = fileContent,
                        Notes = Notes,
                        UserId = userId,
                        status = 3 // Sent to Review
                    };
                    _context.BfileNotes.Add(bfileNote);
                    var bfile = await _context.BFiles.FindAsync(BfileId);
                    bfile.status = 3;
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("Sent", "true");
                    return RedirectToAction(nameof(Index));
                }
              




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

                BfileNote bfileNote = new BfileNote
                {
                    BfileId = BfileId,
                    CurrentFileContent = bfile.fileContent,
                    Notes = "",
                    UserId = ReviewSupervisor,
                    SendUserId = userId,
                    status = 3 // Sent to Review
                };
                _context.BfileNotes.Add(bfileNote);
                bfile.status = 3;
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
                        if (reviewer.IsAdmin != 1)
                        {
                            BfileNote bfileNote1 = new BfileNote
                            {
                                BfileId = BfileId,
                                CurrentFileContent = bfile.fileContent,
                                Notes = "",
                                UserId = reviewer.Id,
                                SendUserId = userId,
                                status = 4 // Sent to Team Review
                            };
                            _context.BfileNotes.Add(bfileNote1);
                            bfile.status = 4;
                        }
                        else
                        {
                            BfileNote bfileNote1 = new BfileNote
                            {
                                BfileId = BfileId,
                                CurrentFileContent = bfile.fileContent,
                                Notes = "",
                                UserId = reviewer.Id,
                                SendUserId = userId,
                                status = 3 // Sent to Supervisor of Reviews Team
                            };
                            _context.BfileNotes.Add(bfileNote1);
                            bfile.status = 3;
                        }
                    }
                }
                else if (Reviewers == 2)
                {
                    for (int i = 0; i < ReviewUsers.Count(); i++)
                    {
                        var reviewer = await _context.ApplicationUsers.FindAsync(ReviewUsers.GetValue(i));

                        if (reviewer.IsAdmin != 1)
                        {
                            BfileNote bfileNote2 = new BfileNote
                            {
                                BfileId = BfileId,
                                CurrentFileContent = bfile.fileContent,
                                Notes = "",
                                UserId = reviewer.Id,
                                SendUserId = userId,
                                status = 4 // Sent to Team Review
                            };
                            _context.BfileNotes.Add(bfileNote2);
                            bfile.status = 4;
                        }
                        else
                        {
                            BfileNote bfileNote2 = new BfileNote
                            {
                                BfileId = BfileId,
                                CurrentFileContent = bfile.fileContent,
                                Notes = "",
                                UserId = reviewer.Id,
                                SendUserId = userId,
                                status = 3 // Sent to Supervisor of Reviews Team
                            };
                            _context.BfileNotes.Add(bfileNote2);
                            bfile.status = 3;
                        }
                    }
                }

                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Sent", "true");
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                return RedirectToAction(nameof(Index));
            }

        }

    }
}
