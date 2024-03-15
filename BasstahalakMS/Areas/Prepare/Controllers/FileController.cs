using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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

        

        //public async Task<IActionResult> Index()
        //{
        //    var files = await _context.BFiles.ToListAsync();
        //    return View(files);
        //}

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(BFile file)
        {
            if (ModelState.IsValid)
            {
                if (file.UploadedFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.UploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.UploadedFile.CopyToAsync(stream);
                    }

                    file.FilePath = filePath;

                    _context.BFiles.Add(file);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            return View(file);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var file = await _context.BFiles.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            return View(file);
        }

        //[Authorize(Roles = "Prepare")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BFile file)
        {
            if (ModelState.IsValid)
            {
                var existingFile = await _context.BFiles.FindAsync(file.Id);
                if (existingFile == null)
                {
                    return NotFound();
                }

                if (file.UploadedFile != null)
                {
                    if (System.IO.File.Exists(existingFile.FilePath))
                    {
                        System.IO.File.Delete(existingFile.FilePath);
                    }
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.UploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.UploadedFile.CopyToAsync(stream);
                    }

                    existingFile.FilePath = filePath;
                }

                existingFile.Name = file.Name;
                existingFile.Description = file.Description;
                existingFile.BookName = file.BookName;
                existingFile.UnitsCount = file.UnitsCount;
                existingFile.LessonsCount = file.LessonsCount;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(file);
        }
    }
}
