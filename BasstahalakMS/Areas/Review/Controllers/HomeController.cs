using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BasstahalakMS.Areas.Review.Controllers
{
    [Authorize(Roles = StaticDetails.Review)]
    [Area(nameof(Review))]
    [Route(nameof(Review) + "/[controller]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
