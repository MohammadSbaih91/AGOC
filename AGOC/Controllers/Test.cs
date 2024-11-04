using Microsoft.AspNetCore.Mvc;

namespace AGOC.Controllers
{
    public class Test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }
    }
}
