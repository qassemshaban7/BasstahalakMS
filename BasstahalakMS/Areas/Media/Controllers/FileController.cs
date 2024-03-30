using BasstahalakMS.Data;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Syncfusion.EJ2.Navigations;
using System.Security.Claims;
using BasstahalakMS.Models;
using Microsoft.AspNetCore.Identity;

namespace BasstahalakMS.Areas.Media.Controllers
{
    [Authorize(Roles = StaticDetails.Media)]
    [Area(nameof(Media))]
    [Route(nameof(Media) + "/[controller]/[action]")]
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public FileController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
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
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.ApplicationUsers.FindAsync(userId);

            if (user.IsAdmin == 1)
            {
                var bFileNotes = await _context.BfileNotes
                    .Where(x => x.SendUserId == userId && (x.status == 3 || x.status == 4 || x.status == 5 || x.status == 6))
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
            else
            {
                var bFileNotes = await _context.BfileNotes
                    .Where(x => x.SendUserId == userId && x.status == 3 || x.status == 4)
                    .Include(c => c.BFile)
                    .ThenInclude(c => c.Book)
                    .Include(c => c.BFile)
                    .ThenInclude(c => c.User)
                    .Where(c => c.BFile.status == 4 || c.BFile.status == 3)
                    .ToListAsync();
                ViewBag.ThisUser = user;
                return View(bFileNotes);
            }
            



            //var bFileNotes = await _context.BfileNotes.Where(x => x.UserId == userId && x.status == 3 || x.status == 4).ToListAsync();
            //var bFiles = await _context.BFiles.Include(x=>x.Book).Include(x=>x.User).Where(x => x.status == 3 || x.status == 4).ToListAsync();
            //if (bFileNotes.Count() == 0)
            //{
            //    bFiles.RemoveAll(x => x.UserId != null);
            //}
            //else
            //{
            //    foreach (var item in bFiles.ToList())
            //    {
            //        foreach (var item1 in bFileNotes)
            //        {
            //            if (item.Id != item1.BfileId)
            //                bFiles.Remove(item);
            //        }
            //    }
            //}

            //return View(bFileNotes);
        }
        public async Task<IActionResult> ShowFile(int id)
		{
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.ApplicationUsers.FindAsync(userId);
            ViewBag.ThisUser = user;

            if (user.IsAdmin == 1)
            {
                var exFile = await _context.BFiles.Include(c => c.Book).Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
                if (exFile == null)
                {
                    return NotFound();
                }

                if (exFile.status == 3 || exFile.status == 4 || exFile.status == 5 || exFile.status == 6)
                {
                    var PerpareMembers = await (from x in _context.ApplicationUsers
                                                join userRole in _context.UserRoles
                                                on x.Id equals userRole.UserId
                                                join role in _context.Roles
                                                on userRole.RoleId equals role.Id
                                                where role.Name == StaticDetails.Prepare
                                                select x)
                                            .ToListAsync();
                    ViewBag.PrepareMemberList = PerpareMembers;

                    var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == exFile.Id).ToListAsync();

                    ViewBag.branches = branches;
                    return View(exFile);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var existingFile = await _context.BFiles.Include(c => c.Book).Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
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
                ViewBag.ReviewAdmins = ReviewAdmins;

                var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
                ViewBag.branches = branches;
                return View(existingFile);
            }
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
                        SendUserId = userId,
                        status = 5 // Back to Prepare
                    };
                    _context.BfileNotes.Add(bfileNote);
                    var bfile = await _context.BFiles.FindAsync(BfileId);
                    bfile.status = 5;
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
                        SendUserId = userId,
                        status = 6 // Sent to Admin
                    };
                    _context.BfileNotes.Add(bfileNote);
                    var bfile = await _context.BFiles.FindAsync(BfileId);
                    bfile.status = 6;
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

        public async Task<IActionResult> ShowToSendToTeam(int id)
        {
            var existingFile = await _context.BFiles.Include(c => c.Book).Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
            if (existingFile == null)
            {
                return NotFound();
            }

            var ReviewUsers = await (from x in _context.ApplicationUsers
                                     join userRole in _context.UserRoles
                                     on x.Id equals userRole.UserId
                                     join role in _context.Roles
                                     on userRole.RoleId equals role.Id
                                     where role.Name == StaticDetails.Review
                                     where x.IsAdmin != 1
                                     select x)
                                 .ToListAsync();
            ViewBag.ReviewUsers = ReviewUsers;

            var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
            ViewBag.branches = branches;
            return View(existingFile);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForReviewTeamMember(int Reviewers, string[] ReviewUsers, int BfileId)
        {
            try
            {
                var bfile = await _context.BFiles.FindAsync(BfileId);

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _context.ApplicationUsers.FindAsync(userId);
                
                if (user.IsAdmin == 1 && bfile.status == 3)
                {
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
                                    ReciveUserId = reviewer.Id,
                                    SendUserId = userId,
                                    status = 4 // Sent to All Team Review
                                };
                                _context.BfileNotes.Add(bfileNote1);
                                bfile.status = 4;
                                bfile.TeamStatus = 0;
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
                                    ReciveUserId = reviewer.Id,
                                    SendUserId = userId,
                                    status = 4 // Sent to specific Member of Team Review
                                };
                                _context.BfileNotes.Add(bfileNote2);
                                bfile.status = 4;
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

        public async Task<IActionResult> ShowToSendToSupervisor(int id)
        {
            var existingFile = await _context.BFiles.Include(c => c.Book).Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
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
            ViewBag.ReviewAdmins = ReviewAdmins;

            var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
            ViewBag.branches = branches;
            return View(existingFile);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForSupervisor(string ReviewSupervisor, int BfileId)    
        {
            try
            {
                var bfile = await _context.BFiles.FindAsync(BfileId);

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _context.ApplicationUsers.FindAsync(userId);

                if (user.IsAdmin == 0 && bfile.status == 4 || user.IsAdmin == 0 && bfile.status == 3 && bfile.TeamStatus == 0 )
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = bfile.fileContent,
                        Notes = "",
                        ReciveUserId = ReviewSupervisor,
                        SendUserId = userId,
                        status = 3 // Sent to Supervisor 
                    };
                    _context.BfileNotes.Add(bfileNote);
                    bfile.status = 3;
                    bfile.TeamStatus = 1;

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

        //public async Task<IActionResult> SendForTeam(int id)  
        //{
        //if (HttpContext.Session.GetString("created") != null)
        //{
        //    ViewBag.created = true;
        //    HttpContext.Session.Remove("created");
        //}
        //if (HttpContext.Session.GetString("Sent") != null)
        //{
        //    ViewBag.Sent = true;
        //    HttpContext.Session.Remove("Sent");
        //}

        //var existingFile = await _context.BFiles.Include(c => c.Book).Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
        //if (existingFile == null)
        //{
        //    return NotFound();
        //}

        //var usersWithPermission = _userManager.GetUsersInRoleAsync(StaticDetails.Review).Result;
        //var idsWithPermission = usersWithPermission.Select(u => u.Id);
        //var users = await _context.ApplicationUsers.Where(u => idsWithPermission.Contains(u.Id) && u.IsAdmin == 0).ToListAsync();

        //var files = await _context.BFiles.Include(x => x.Book).Include(c => c.User).Where(f => f.status == 3).ToListAsync();

        //var model = new Tuple<List<ApplicationUser>, List<BFile>>(users, files);

        //var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
        //ViewBag.branches = branches;

        //ViewBag.Users = users;
        //return View(model);

        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SendForTeam(String sendUserId, string fileContent, string Notes, int Prepare, int BfileId)
        //{
        //    try
        //    {

        //        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //        if (Prepare == 3)
        //        {
        //            BfileNote bfileNote = new BfileNote
        //            {
        //                BfileId = BfileId,
        //                CurrentFileContent = fileContent,
        //                Notes = Notes,
        //                UserId = userId,
        //                SendUserId = sendUserId,
        //                status = 4 // Send For Team
        //            };
        //            _context.BfileNotes.Add(bfileNote);
        //            var bfile = await _context.BFiles.FindAsync(BfileId);
        //            bfile.status = 4;
        //            await _context.SaveChangesAsync();
        //            HttpContext.Session.SetString("Sent", "true");
        //            return RedirectToAction(nameof(Index));
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {

        //        return RedirectToAction(nameof(Index));
        //    }
        //}

    }
}
