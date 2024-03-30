using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Azure.Core.HttpHeader;


namespace BasstahalakMS.Areas.Prepare.Controllers
{
    [Authorize(Roles = StaticDetails.Prepare)]
    [Area(nameof(Prepare))]
    [Route(nameof(Prepare) + "/[controller]/[action]")]
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
            if (HttpContext.Session.GetString("created") != null)
            {
                ViewBag.created = true;
                HttpContext.Session.Remove("created");
            }

            if (HttpContext.Session.GetString("Sent") != null)
            {
                ViewBag.Sent = true;
                HttpContext.Session.Remove("Sent");
            }
            if (HttpContext.Session.GetString("updated") != null)
            {
                ViewBag.updated = true;
                HttpContext.Session.Remove("updated");
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.ApplicationUsers.FindAsync(userId);

            //var files = await _context.BFiles
            //    .Include(x => x.Book)
            //    .Where(x => x.UserId == userId && (x.status == 0 || x.status == 1 || x.status == 2 || x.status == 5))
            //    .ToListAsync();


            var bFileNotes = await _context.BfileNotes
                .Where(x => x.ReciveUserId == userId &&
                            (x.BFile.status == 0 || x.BFile.status == 1 || x.BFile.status == 2 || x.BFile.status == 5))
                .Include(c => c.BFile)
                    .ThenInclude(c => c.Book)
                .Include(c => c.BFile.User)
                .ToListAsync();


            var distinctBFiles = bFileNotes.GroupBy(x => x.BFile.Id)
                                            .Select(group => group.First())
                                            .ToList();

            ViewBag.ThisUser = user;
            return View(distinctBFiles);
        }

        public IActionResult Upload()
        {
            var books = _context.Books.ToList();
            ViewBag.Books = books;
            var branches = _context.Branches.ToList();
            ViewBag.Branches = branches;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(BFile bFile/*, IFormFile uploadedFile*/)
        {
           try
            {
                //if (uploadedFile != null)
                //{
                    //string uploadsFolder = Path.Combine("files");
                    //string uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, uploadsFolder);
                    //if (!Directory.Exists(uploadsFolderPath))
                    //{
                    //    Directory.CreateDirectory(uploadsFolderPath);
                    //}

                    //string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                    //string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await uploadedFile.CopyToAsync(stream);
                    //}

                    string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    var file = new BFile
                    {
                        Name = bFile.Name,
                        Description = bFile.Description,
                        fileContent = bFile.fileContent,
                        //FilePath = Path.Combine(uploadsFolder, uniqueFileName),
                        BookId = bFile.BookId,
                        //BranchName = branchName,
                        //UnitsCount = unitsCount,
                        //LessonsCount = lessonsCount,
                        UserId = userId
                    };

                    _context.BFiles.Add(file);
                    await _context.SaveChangesAsync();
                    var branchesInDB = await _context.Branches.ToListAsync();
                    foreach (var branch in branchesInDB)
                    {
                        string name = "check_" + branch.Id;
                        string chkValue = Request.Form[name].ToString();
                        if(chkValue == "1")
                        {
                            string noOfUnitsName = "noUnits_" + @branch.Id;
                            string noOfLessonsName = "noLessons_" + @branch.Id;
                            string noOfUnits = Request.Form[noOfUnitsName].ToString();
                            string noOfLessons = Request.Form[noOfLessonsName].ToString();
                            var fileBranch = new FileBranch
                            {
                                BFileId = file.Id,
                                BranchId = branch.Id,
                                LessonsCount = Convert.ToInt32(noOfLessons),
                                UnitsCount = Convert.ToInt32(noOfUnits),


                            };
                            _context.FileBranches.Add(fileBranch);
                            await _context.SaveChangesAsync();
                        }
                    }
                    HttpContext.Session.SetString("created", "true");
                    return RedirectToAction(nameof(Index));
                //}
                //else
                //{
                //    var books = _context.Books.ToList();
                //    ViewBag.Books = books;
                //    var branches = _context.Branches.ToList();
                //    ViewBag.Branches = branches;
                //    return View();
                //}
            }
            catch (Exception ex)
            {
                var books = _context.Books.ToList();
                ViewBag.Books = books;
                var branches = _context.Branches.ToList();
                ViewBag.Branches = branches;
                return View();
            }
            
        }

        public async Task<IActionResult> Edit(int id)
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

            var bfileNote = await _context.BfileNotes.Include(x=>x.BFile)
                .Include(x=>x.User)
                .OrderBy(e => e.CreationDate)
                .LastOrDefaultAsync(x => x.status == existingFile.status && x.SendUserId != userId);

            ViewBag.bfileNote = bfileNote;
            return View(existingFile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BFile bFile)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var existingFile = await _context.BFiles.FindAsync(bFile.Id);
                if(existingFile != null)
                {
                    existingFile.Name = bFile.Name;
                    existingFile.Description = bFile.Description;
                    existingFile.fileContent = Request.Form["fileContent"].ToString();
                    existingFile.BookId = bFile.BookId;
                    bool updated = true;
                    if(existingFile.status == 2  || existingFile.status == 0 || existingFile.status == 6) {
                        //existingFile.status = 1;
                        BfileNote bfileNote = new BfileNote
                        {
                            BfileId = bFile.Id,
                            CurrentFileContent = Request.Form["fileContent"].ToString(),
                            Notes = "",
                            SendUserId = userId,
                            status = 0 // Back to Admin
                        };
                        _context.BfileNotes.Add(bfileNote);
                        updated = false;
                    }  
                    //else if(existingFile.status == 4)
                    //{
                    //    existingFile.status = 3;
                    //    BfileNote bfileNote = new BfileNote
                    //    {
                    //        BfileId = bFile.Id,
                    //        CurrentFileContent = Request.Form["fileContent"].ToString(),
                    //        Notes = "",
                    //        UserId = userId,
                    //        status = 3 // Back to Review
                    //    };
                    //    _context.BfileNotes.Add(bfileNote);
                    //    updated = false;
                    //}  
                    await _context.SaveChangesAsync();

                    var currentBranches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
                    foreach (var item in currentBranches)
                    {
                        _context.FileBranches.Remove(item);
                    }
                    await _context.SaveChangesAsync();
                    var branchesInDB = await _context.Branches.ToListAsync();
                    foreach (var branch in branchesInDB)
                    {
                        string name = "check_" + branch.Id;
                        string chkValue = Request.Form[name].ToString();
                        if (chkValue == "1")
                        {
                            string noOfUnitsName = "noUnits_" + @branch.Id;
                            string noOfLessonsName = "noLessons_" + @branch.Id;
                            string noOfUnits = Request.Form[noOfUnitsName].ToString();
                            string noOfLessons = Request.Form[noOfLessonsName].ToString();
                            var fileBranch = new FileBranch
                            {
                                BFileId = bFile.Id,
                                BranchId = branch.Id,
                                LessonsCount = Convert.ToInt32(noOfLessons),
                                UnitsCount = Convert.ToInt32(noOfUnits),


                            };
                            _context.FileBranches.Add(fileBranch);
                            await _context.SaveChangesAsync();
                        }
                    }
                    if(updated == false)
                        HttpContext.Session.SetString("Sent", "true");
                    else
                        HttpContext.Session.SetString("updated", "true");
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
                
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        //public async Task<IActionResult> SendForReview(int? id)
        //{
        //    try
        //    {
        //        var bFile = await _context.BFiles.FindAsync(id);
        //        if (bFile == null)
                //    return NotFound();

                //if(bFile.status == 0)
                    //bFile.status = 1; // Under Super Admin Review
                //else if(bFile.status == 2)
                    //bFile.status = 1; // back to  Super Admin Review
                //else if(bFile.status == 4)
                //    bFile.status = 3; // back to  Review
        //        await _context.SaveChangesAsync();
        //        HttpContext.Session.SetString("Sent", "true");
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        public async Task<IActionResult> ShowToSendToReviwSupervisor(int id)
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
        public async Task<IActionResult> SendForReviwSupervisor(string ReviewSupervisor, int BfileId)
        {
            try
            {
                var bfile = await _context.BFiles.FindAsync(BfileId);

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _context.ApplicationUsers.FindAsync(userId);

                if (bfile.status == 5)
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

        public async Task<IActionResult> ShowFile(int id)
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
        public async Task<IActionResult> SendForAdminOrReview(string fileContent, string Notes, int Prepare, int BfileId)   
        {
            try
            {

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var bF = _context.BFiles.Find(BfileId);

                if (bF.status == 0)
                {
                    if (Prepare == 0)
                    {
                        var SuperAdmin = await (from x in _context.ApplicationUsers
                                                join userRole in _context.UserRoles
                                                on x.Id equals userRole.UserId
                                                join role in _context.Roles
                                                on userRole.RoleId equals role.Id
                                                where role.Name == StaticDetails.SuperAdmin
                                                select x)
                                            .SingleOrDefaultAsync();

                        BfileNote bfileNote = new BfileNote
                        {
                            BfileId = BfileId,
                            CurrentFileContent = fileContent,
                            Notes = Notes,
                            ReciveUserId = SuperAdmin.Id,
                            SendUserId = userId,
                            status = 1 // Sent to Admin
                        };
                        _context.BfileNotes.Add(bfileNote);
                        var bfile = await _context.BFiles.FindAsync(BfileId);
                        bfile.status = 1;
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

                if (bfile.status == 0 || bfile.status == 2 )
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = bfile.fileContent,
                        Notes = "",
                        ReciveUserId = ReviewSupervisor,
                        SendUserId = userId,
                        status = 3 // Sent to Review
                    };
                    _context.BfileNotes.Add(bfileNote);
                    bfile.status = 3;
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
                                status = 3 // Sent to Team Review
                            };
                            _context.BfileNotes.Add(bfileNote1);
                            bfile.status = 3;
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
                                    status = 3 // Sent to Supervisor of team Reviews
                                };
                                _context.BfileNotes.Add(bfileNote2);
                                bfile.status = 3;
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
                                    status = 3 // Sent to Team Review
                                };
                                _context.BfileNotes.Add(bfileNote2);
                                bfile.status = 3;
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
