using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BasstahalakMS.Areas.Printing.Controllers  
{
    [Authorize(Roles = StaticDetails.Printing)]
    [Area(nameof(Printing))]
    [Route(nameof(Printing) + "/[controller]")] 
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
