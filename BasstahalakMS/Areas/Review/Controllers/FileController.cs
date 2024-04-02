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

namespace BasstahalakMS.Areas.Review.Controllers
{
    [Authorize(Roles = StaticDetails.Review)]
    [Area(nameof(Review))]
    [Route(nameof(Review) + "/[controller]/[action]")]
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
                    .Where(x => x.SendUserId == userId && (x.status == 3 || x.status == 4 || x.BFile.status == -1 || x.BFile.status == 6))
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
                    .Where(x => x.ReciveUserId == userId && x.status == 3 || x.status == 4 || x.BFile.status == -1)
                    .Include(c => c.BFile)
                    .ThenInclude(c => c.Book)
                    .Include(c => c.BFile)
                    .ThenInclude(c => c.User)
                    .Where(c => c.BFile.status == 4 || c.BFile.status == 3)
                    .ToListAsync();
                


                var distinctBFiles = bFileNotes.GroupBy(x => x.BFile.Id)
                                                .Select(group => group.First())
                                                .ToList();
                ViewBag.ThisUser = user;
                return View(distinctBFiles);
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
                    UserId = userId,
                    status = -1
                };

                _context.BFiles.Add(file);
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
                            BFileId = file.Id,
                            BranchId = branch.Id,
                            LessonsCount = Convert.ToInt32(noOfLessons),
                            UnitsCount = Convert.ToInt32(noOfUnits),


                        };

                        var bfileNote = new BfileNote
                        {  
                            BfileId = file.Id,
                            CurrentFileContent = file.fileContent,
                            Notes = " ",
                            SendUserId = userId,
                            ReciveUserId = userId,
                            status = -1
                        };
                        _context.BfileNotes.Add(bfileNote);

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

            if(existingFile.status == -1)
            {
                var bfileNotess = await _context.BfileNotes
                                .Where(p => p.BfileId == existingFile.Id  && p.status == -1 && p.BFile.UserId == userId)
                                .Include(x => x.BFile)
                                .Include(x => x.User)
                                .ToListAsync();

                ViewBag.bfileNote = bfileNotess;
                return View(existingFile);
            }

            var bfileNote = await _context.BfileNotes.Include(x => x.BFile)
                .Include(x => x.User)
                .OrderBy(e => e.CreationDate)
                .LastOrDefaultAsync(x => x.status == existingFile.status && x.ReciveUserId != userId);
            
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
                if (existingFile != null)
                {
                    existingFile.Name = bFile.Name;
                    existingFile.Description = bFile.Description;
                    existingFile.fileContent = Request.Form["fileContent"].ToString();
                    existingFile.BookId = bFile.BookId;
                    bool updated = true;
                    if (existingFile.status == 3 || existingFile.status == 4)
                    {
                        BfileNote bfileNote = new BfileNote
                        {
                            BfileId = bFile.Id,
                            CurrentFileContent = Request.Form["fileContent"].ToString(),
                            Notes = "",
                            ReciveUserId = userId,
                            status = existingFile.status
                        };
                        existingFile.status = existingFile.status;
                        _context.BfileNotes.Add(bfileNote);
                        updated = false;
                    }
                    
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
                    if (updated == false)
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

        public async Task<IActionResult> ShowFile(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.ApplicationUsers.FindAsync(userId);
            ViewBag.ThisUser = user;

            //if (user.IsAdmin == 1)
            //{
                var exFile = await _context.BFiles.Include(c => c.Book).Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
                if (exFile == null)
                {
                    return NotFound();
                }

                if (exFile.status == 3 || exFile.status == 4 || exFile.status == 5 || exFile.status == 6 || exFile.status == -1) 
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
            //}
            //else
            //{
            //    var existingFile = await _context.BFiles.Include(c => c.Book).Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
            //    if (existingFile == null)
            //    {
            //        return NotFound();
            //    }

            //    var ReviewAdmins = await (from x in _context.ApplicationUsers
            //                              join userRole in _context.UserRoles
            //                              on x.Id equals userRole.UserId
            //                              join role in _context.Roles
            //                              on userRole.RoleId equals role.Id
            //                              where role.Name == StaticDetails.Review
            //                              where x.IsAdmin == 1
            //                              select x)
            //                         .ToListAsync();
            //    ViewBag.ReviewAdmins = ReviewAdmins;

            //    var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
            //    ViewBag.branches = branches;
            //    return View(existingFile);
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForReview(string fileContent, string Notes, int Prepare, int BfileId)
        {
            try
            {

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (Prepare == 1)
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = fileContent,
                        Notes = Notes,
                        SendUserId = userId,
                        //ReciveUserId = ReceiverId,
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
                        SendUserId = userId,
                        ReciveUserId = SuperAdmin.Id,
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

                if (user.IsAdmin == 1 && bfile.status == 3 || bfile.status == -1)
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

                if (user.IsAdmin == 0 && bfile.status == 4 || user.IsAdmin == 0 && bfile.status == 3 && bfile.TeamStatus == 0 || bfile.status == -1 && bfile.UserId == userId)
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
                                .Where(p => p.BfileId == existingFile.Id && p.status == -1 && p.BFile.UserId == userId)
                                .Include(x => x.BFile)
                                .Include(x => x.User)
                                .ToListAsync();

                ViewBag.bfileNote = bfileNotess;
                return View(existingFile);
            }

            var bfileNote = await _context.BfileNotes.Include(x => x.BFile)
                .Include(x => x.User)
                .OrderBy(e => e.CreationDate)
                .LastOrDefaultAsync(x => x.status == existingFile.status && x.ReciveUserId != userId);

            ViewBag.bfileNote = bfileNote;
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
                if (existingFile != null)
                {
                    //existingFile.Name = existingFile.Name;
                    //existingFile.Description = existingFile.Description;
                    //existingFile.fileContent = Request.Form["fileContent"].ToString();
                    //existingFile.BookId = existingFile.BookId;
                    bool updated = true;
                    if (existingFile.status == 3 || existingFile.status == -1 || existingFile.status == 4)
                    {
                        BfileNote bfileNote = new BfileNote
                        {
                            BfileId = bFile.Id,
                            CurrentFileContent = Request.Form["fileContent"].ToString(),
                            Notes = "",
                            SendUserId = userId,
                            ReciveUserId = userId,
                            status = 10

                        };
                        existingFile.status = 10;
                        _context.BfileNotes.Add(bfileNote);
                        updated = false;
                    }

                    await _context.SaveChangesAsync();

                    //var currentBranches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
                    //foreach (var item in currentBranches)
                    //{
                    //    _context.FileBranches.Remove(item);
                    //}
                    //await _context.SaveChangesAsync();
                    //var branchesInDB = await _context.Branches.ToListAsync();
                    //foreach (var branch in branchesInDB)
                    //{
                    //    string name = "check_" + branch.Id;
                    //    string chkValue = Request.Form[name].ToString();
                    //    if (chkValue == "1")
                    //    {
                    //        string noOfUnitsName = "noUnits_" + @branch.Id;
                    //        string noOfLessonsName = "noLessons_" + @branch.Id;
                    //        string noOfUnits = Request.Form[noOfUnitsName].ToString();
                    //        string noOfLessons = Request.Form[noOfLessonsName].ToString();
                    //        var fileBranch = new FileBranch
                    //        {
                    //            BFileId = bFile.Id,
                    //            BranchId = branch.Id,
                    //            LessonsCount = Convert.ToInt32(noOfLessons),
                    //            UnitsCount = Convert.ToInt32(noOfUnits),


                    //        };
                    //        _context.FileBranches.Add(fileBranch);
                    //        await _context.SaveChangesAsync();
                    //    }
                    //}
                    if (updated == false)
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
