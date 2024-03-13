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
        public IActionResult Index()
        {
            return View();
        }
    }
}
