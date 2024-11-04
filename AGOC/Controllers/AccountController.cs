using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace AGOC.Controllers
{
    public class AccountController : Controller
    {
        private readonly LdapAuthenticationService _ldapService;
        private readonly ILogger<AccountController> _logger;
        private readonly string? _userId;
        private readonly string? _username;
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration, LdapAuthenticationService ldapAuthenticationService, IHttpContextAccessor httpContextAccessor, ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _ldapService = ldapAuthenticationService;
            _logger = logger;
            _userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _username = httpContextAccessor.HttpContext?.User.Identity?.Name;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("User {Username} is already authenticated. Redirecting to Vehicles Index.", _username);
                return RedirectToAction("Index", "Vehicles");
            }

            ViewData["ReturnUrl"] = returnUrl;
            _logger.LogInformation("Displaying login page.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                //else
                //{
                //    return RedirectToAction("Index", "Test");
                //}
                var validationResult = await _ldapService.ValidateUserAsync(username, password);

                if (validationResult.IsValid)
                {
                    _logger.LogInformation("User {Username} successfully authenticated.", username);

                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

                    if (validationResult.Roles != null)
                    {
                        claims.AddRange(validationResult.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.Now.AddMinutes(60)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    _logger.LogInformation("User {Username} signed in successfully.", username);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Vehicles");
                    }
                }
                else
                {
                    _logger.LogWarning("Failed login attempt for user {Username}. Error: {ErrorMessage}", username, validationResult.ErrorMessage);
                    ViewData["Message"] = validationResult.ErrorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to log in user {Username}.", username);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User {Username} is logging out.", _username);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User {Username} logged out successfully.", _username);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            _logger.LogWarning("User {Username} attempted to access a denied resource.", _username);
            return View();
        }
    }
}