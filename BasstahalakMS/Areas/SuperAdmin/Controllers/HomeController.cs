using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BasstahalakMS.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticDetails.SuperAdmin)]
    [Area(nameof(SuperAdmin))]
    [Route(nameof(SuperAdmin) + "/[controller]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
