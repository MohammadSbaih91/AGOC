using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;

namespace AGOC.Web.Controllers
{
    [Authorize]
    public class SmslogController : Controller
    {
        private readonly ISmslogManager _smslogManager;

        public SmslogController(ISmslogManager smslogManager)
        {
            _smslogManager = smslogManager;
        }

        // GET: Smslog
        public async Task<IActionResult> Index()
        {
            try
            {
                var smslogs = await _smslogManager.GetAllSmslogesAsync();
                return View(smslogs); // This will return the view with the list of SMS logs
            }
            catch (Exception ex)
            {
                // Log error or handle it as needed
                ViewData["ErrorMessage"] = "An error occurred while fetching the SMS logs.";
                return View("Error");
            }
        }
    }
}