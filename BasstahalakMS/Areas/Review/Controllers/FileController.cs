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
            var bFileNotes = await _context.BfileNotes.Where(x => x.UserId == userId && x.status == 3 || x.status == 4).ToListAsync();
            var bFiles = await _context.BFiles.Include(x=>x.Book).Include(x=>x.User).Where(x => x.status == 3 || x.status == 4).ToListAsync();
            if (bFileNotes.Count() == 0)
            {
                bFiles.RemoveAll(x => x.UserId != null);
            }
            else
            {
                foreach (var item in bFiles.ToList())
                {
                    foreach (var item1 in bFileNotes)
                    {
                        if (item.Id != item1.BfileId)
                            bFiles.Remove(item);
                    }
                }
            }


            //string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;    
            
            return View(bFiles);
        }
		public async Task<IActionResult> ShowFile(int id)
		{
			var existingFile = await _context.BFiles.Include(c=>c.Book).Include(c=>c.User).FirstOrDefaultAsync(x=>x.Id == id);
			if (existingFile == null)
			{
				return NotFound();
			}

            if(existingFile.status == 3)
            {
                var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
                ViewBag.branches = branches;
                return View(existingFile);
            }

            return RedirectToAction(nameof(Index));
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForReview(string fileContent , string Notes, int Prepare, int BfileId)
        {
            try
            {
                
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if(Prepare == 3)
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = fileContent,
                        Notes = Notes,
                        UserId = userId,
                        status = 4 // Back to Prepare
                    };
                    _context.BfileNotes.Add(bfileNote);
                    var bfile = await _context.BFiles.FindAsync(BfileId);
                    bfile.status = 4;
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
                        status = 7 // Sent to Admin
                    };
                    _context.BfileNotes.Add(bfileNote);
                    var bfile = await _context.BFiles.FindAsync(BfileId);
                    bfile.status = 7;
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

        public async Task<IActionResult> SendForTeam(int id)  
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

            var existingFile = await _context.BFiles.Include(c => c.Book).Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
            if (existingFile == null)
            {
                return NotFound();
            }

            var usersWithPermission = _userManager.GetUsersInRoleAsync(StaticDetails.Review).Result;
            var idsWithPermission = usersWithPermission.Select(u => u.Id);
            var users = await _context.ApplicationUsers.Where(u => idsWithPermission.Contains(u.Id) && u.IsAdmin == 0).ToListAsync();

            var files = await _context.BFiles.Include(x => x.Book).Include(c => c.User).Where(f => f.status == 3).ToListAsync();

            var model = new Tuple<List<ApplicationUser>, List<BFile>>(users, files);

            var branches = await _context.FileBranches.Include(x => x.Branch).Where(x => x.BFileId == existingFile.Id).ToListAsync();
            ViewBag.branches = branches;
            
            ViewBag.Users = users;
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForTeam(String sendUserId, string fileContent, string Notes, int Prepare, int BfileId)
        {
            try
            {

                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (Prepare == 3)
                {
                    BfileNote bfileNote = new BfileNote
                    {
                        BfileId = BfileId,
                        CurrentFileContent = fileContent,
                        Notes = Notes,
                        UserId = userId,
                        SendUserId = sendUserId,
                        status = 4 // Send For Team
                    };
                    _context.BfileNotes.Add(bfileNote);
                    var bfile = await _context.BFiles.FindAsync(BfileId);
                    bfile.status = 4;
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
