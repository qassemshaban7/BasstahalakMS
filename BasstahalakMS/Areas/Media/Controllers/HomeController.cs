using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BasstahalakMS.Areas.Media.Controllers
{
    [Authorize(Roles = StaticDetails.Media)]
    [Area(nameof(Media))]
    [Route(nameof(Media) + "/[controller]")]
    public class HomeController : Controller
    {
            public IActionResult Index()
            {
                return View();
            }
        
    }
}
