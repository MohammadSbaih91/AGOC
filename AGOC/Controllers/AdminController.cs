using Microsoft.AspNetCore.Mvc;

namespace AGOC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
