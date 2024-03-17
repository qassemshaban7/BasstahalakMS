using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


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
            var files = await _context.BFiles.ToListAsync();
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
        public async Task<IActionResult> Upload(string name, string description, string bookName, string branchName, int unitsCount, int lessonsCount, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    string uploadsFolder = Path.Combine("files");
                    string uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, uploadsFolder);
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(stream);
                    }

                    string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    var file = new BFile
                    {
                        Name = name,
                        Description = description,
                        FilePath = Path.Combine(uploadsFolder, uniqueFileName),
                        BookName = bookName,
                        BranchName = branchName,
                        UnitsCount = unitsCount,
                        LessonsCount = lessonsCount,
                        UserId = userId
                    };

                    _context.BFiles.Add(file);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var existingFile = await _context.BFiles.FindAsync(id);
            if (existingFile == null)
            {
                return NotFound();
            }

            var editViewModel = new EditBFileViewModel
            {
                Id = existingFile.Id,
                Name = existingFile.Name,
                Description = existingFile.Description,
                BookName = existingFile.BookName,
                BranchName = existingFile.BranchName,
                UnitsCount = existingFile.UnitsCount,
                LessonsCount = existingFile.LessonsCount
            };

            var books = _context.Books.ToList(); 
            ViewBag.Books = books;

            var branches = _context.Branches.ToList();
            ViewBag.Branches = branches;

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingFile = await _context.BFiles.FindAsync(model.Id);
                if (existingFile == null)
                {
                    return NotFound();
                }

                if (model.UploadedFile != null)
                {
                    if (System.IO.File.Exists(existingFile.FilePath))
                    {
                        System.IO.File.Delete(existingFile.FilePath);
                    }

                    string uploadsFolder = "files";
                    string uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, uploadsFolder);
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                    string wwwRootPath = Path.GetFullPath(_hostingEnvironment.WebRootPath);

                    existingFile.FilePath = Path.GetRelativePath(wwwRootPath, filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UploadedFile.CopyToAsync(stream);
                    }
                }

                existingFile.Name = model.Name;
                existingFile.Description = model.Description;
                existingFile.BookName = model.BookName;
                existingFile.BranchName = model.BranchName;
                existingFile.UnitsCount = model.UnitsCount;
                existingFile.LessonsCount = model.LessonsCount;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

    }
}
