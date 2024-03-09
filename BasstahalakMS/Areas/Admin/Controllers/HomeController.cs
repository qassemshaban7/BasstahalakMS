using BasstahalakMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BasstahalakMS.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticDetails.Admin)]
    [Area(nameof(Admin))]
    [Route(nameof(Admin) + "/[controller]")]
    public class HomeController : Controller
    {
            public IActionResult Index()
            {
                return View();
            }
        
    }
}
