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

namespace BasstahalakMS.Areas.Review.Controllers
{
    [Authorize(Roles = StaticDetails.Review)]
    [Area(nameof(Review))]
    [Route(nameof(Review) + "/[controller]/[action]")]
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
            var user = await _context.ApplicationUsers.FindAsync(userId);

            var pdfs = await _context.pdfNotes
                .Where(c => c.ReciveUserId == userId)
                .Include(c => c.User)
                .Include(x => x.PdfFile)
                .Include(c => c.User)
                .ToListAsync();

            var distinctpdfs = pdfs.GroupBy(x => x.PdfFile.Id)
                                .Select(group => group.First())
                                .ToList();

            return View(distinctpdfs);
        }

        public async Task<IActionResult> ShowFile(int id)
        {
            var existingFile = await _context.PdfFiles.Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);

            if (existingFile == null)
            {
                return NotFound();
            }

            var PerpareMembers = await (from x in _context.ApplicationUsers
                                     join userRole in _context.UserRoles
                                     on x.Id equals userRole.UserId
                                     join role in _context.Roles
                                     on userRole.RoleId equals role.Id
                                     where role.Name == StaticDetails.Media
                                     select x)
                                 .ToListAsync();

            ViewBag.PrepareMemberList = PerpareMembers;

            return View(existingFile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForReview(string MediaId, string Notes, int Prepare, int BfileId)
        {
            try
            {
                var bF = await _context.PdfFiles.FindAsync(BfileId);
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (Prepare == 1)
                {
                    pdfNote pdfnote = new pdfNote
                    {
                        PdfId = BfileId,
                        Description = Notes,
                        ReciveUserId = MediaId  // Sent to Media
                    };
                    _context.pdfNotes.Add(pdfnote);
                    bF.status = 2;
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

                    pdfNote pdfnote = new pdfNote
                    {
                        PdfId = BfileId,
                        Description = Notes,
                        ReciveUserId = SuperAdmin.Id   // Sent to Admin
                    };
                    _context.pdfNotes.Add(pdfnote);
                    bF.status = 1;
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
    }
}