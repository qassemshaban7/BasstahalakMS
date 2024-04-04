using BasstahalakMS.Data;
using BasstahalakMS.Models;
using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using static Azure.Core.HttpHeader;

namespace BasstahalakMS.Areas.Media.Controllers
{
    [Authorize(Roles = StaticDetails.Media)]
    [Area(nameof(Media))]
    [Route(nameof(Media) + "/[controller]/[action]")]
    public class PDFController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PDFController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
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

            var pdfs = await _context.PdfFiles.Where(x => x.UserId == userId).Include(c => c.User).ToListAsync();
            return View(pdfs);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(PdfFile pdf)
        {
           try{
                if (pdf.pdfFile != null)
                {
                    string uploadsFolder = Path.Combine("pdffiles");
                    string uniqueFilePath = Path.Combine(_hostingEnvironment.WebRootPath, uploadsFolder);
                    if (!Directory.Exists(uniqueFilePath))
                    {
                        Directory.CreateDirectory(uniqueFilePath);
                    }


                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + pdf.pdfFile.FileName;
                    string filePath = Path.Combine(uniqueFilePath, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await pdf.pdfFile.CopyToAsync(fileStream);
                    }

                    string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var file = new PdfFile
                    {
                        Name = pdf.Name,
                        Description = pdf.Description,
                        PDFPath = uniqueFileName,
                        status = 0,
                        UserId = userId
                    };

                    _context.PdfFiles.Add(file);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View(pdf);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var pdf = await _context.PdfFiles.FindAsync(id);
            if (pdf == null)
            {
                return NotFound();
            }
            return View(pdf);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PdfFile pdf)
        {
            if (ModelState.IsValid)
            {
                var existingFile = await _context.PdfFiles.FindAsync(pdf.Id);
                if (existingFile == null)
                {
                    return NotFound();
                }

                if (pdf.pdfFile != null)
                {
                    if (System.IO.File.Exists(existingFile.PDFPath))
                    {
                        System.IO.File.Delete(existingFile.PDFPath);
                    }
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "pdffiles");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + pdf.pdfFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await pdf.pdfFile.CopyToAsync(stream);
                    }

                    existingFile.PDFPath = uniqueFileName;
                }

                existingFile.Name = pdf.Name;
                existingFile.Description = pdf.Description;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(pdf);
        }

        public async Task<IActionResult> ShowFile(int id)
        {
            var existingFile = await _context.PdfFiles.Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
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

            return View(existingFile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForAdminOrReview(string Notes, int Prepare, int pdfId)
        {
            try
            {

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var bF = _context.PdfFiles.Find(pdfId);

                if (true)
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

                        pdfNote pdfnote = new pdfNote
                        {
                            PdfId = pdfId,
                            Description = Notes,
                            ReciveUserId = SuperAdmin.Id   // Sent to Admin
                        };
                        _context.pdfNotes.Add(pdfnote);
                        bF.status = 1;
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("Sent", "true");
                        return RedirectToAction(nameof(Index));
                    }
                    else if(Prepare == 5)
                    {
                        pdfNote pdfnote = new pdfNote
                        {
                            PdfId = pdfId,
                            Description = Notes    // Sent to Review
                        };
                        _context.pdfNotes.Add(pdfnote);
                        bF.status = 3;
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
        public async Task<IActionResult> SendForReviewTeam(string ReviewSupervisor, int Reviewers, string[] ReviewUsers, int pdfId)
        {
            try
            {

                var bfile = await _context.PdfFiles.FindAsync(pdfId);

                    pdfNote pdfnote = new pdfNote
                    {
                        PdfId = pdfId, 
                        Description = "",
                        ReciveUserId = ReviewSupervisor  // Sent to Review
                    };
                    _context.pdfNotes.Add(pdfnote);
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
                            pdfNote pdfnote1 = new pdfNote
                            {
                                PdfId = pdfId,
                                Description = "",
                                ReciveUserId = reviewer.Id   // Sent to Review
                            };
                            _context.pdfNotes.Add(pdfnote1);
                            bfile.status = 3;

                        }
                    }
                    else if (Reviewers == 2)
                    {
                        for (int i = 0; i < ReviewUsers.Count(); i++)
                        {
                            var reviewer = await _context.ApplicationUsers.FindAsync(ReviewUsers.GetValue(i));
                            pdfNote pdfnote1 = new pdfNote
                            {
                                PdfId = pdfId,
                                Description = "",
                                ReciveUserId = reviewer.Id   // Sent to Review
                            };
                            _context.pdfNotes.Add(pdfnote1);
                            bfile.status = 3;
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