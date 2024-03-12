using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Hosting;

namespace BasstahalakMS.Areas.Prepare.Controllers
{
    [Authorize(Roles = StaticDetails.Prepare)]
    [Area(nameof(Prepare))]
    [Route(nameof(Prepare) + "/[controller]")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
        return View();
        }

        public IActionResult ShowFiles()
        {
            var files = _context.EFiles.ToList();
            return View(files);
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [Authorize(Roles = "Prepare")]
        [HttpPost]
        public async Task<IActionResult> UploadFile(eFile file)
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
                    file.Name = file.Name;
                    file.Description = file.Description; 

                    _context.EFiles.Add(file);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            return View(file);
        }


    }
}
