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
           
            var files = await _context.BFiles.Include(x => x.Book).Where(x => x.UserId == userId ).ToListAsync();
            return View(files);
            
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
                .LastOrDefaultAsync(x => x.status == existingFile.status && x.UserId != userId);

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
                    if(existingFile.status == 2) {
                        existingFile.status = 1;
                        BfileNote bfileNote = new BfileNote
                        {
                            BfileId = bFile.Id,
                            CurrentFileContent = Request.Form["fileContent"].ToString(),
                            Notes = "",
                            UserId = userId,
                            status = 1 // Back to Admin
                        };
                        _context.BfileNotes.Add(bfileNote);
                        updated = false;
                    }  
                    else if(existingFile.status == 4)
                    {
                        existingFile.status = 3;
                        BfileNote bfileNote = new BfileNote
                        {
                            BfileId = bFile.Id,
                            CurrentFileContent = Request.Form["fileContent"].ToString(),
                            Notes = "",
                            UserId = userId,
                            status = 3 // Back to Review
                        };
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

        public async Task<IActionResult> SendForReview(int? id)
        {
            try
            {
                var bFile = await _context.BFiles.FindAsync(id);
                if (bFile == null)
                    return NotFound();

                if(bFile.status == 0)
                    bFile.status = 1; // Under Super Admin Review
                else if(bFile.status == 2)
                    bFile.status = 1; // back to  Super Admin Review
                else if(bFile.status == 4)
                    bFile.status = 3; // back to  Review
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
