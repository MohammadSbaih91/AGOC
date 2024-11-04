using AGOC.Services.Interface;
using AGOC.ViewModels;

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
        private readonly IConfiguration _configuration;
        private readonly IUserAuthenticationService _userAuthService; // For non-LDAP authentication
        private readonly string? _userId;
        private readonly string? _username;

        public AccountController(
            IConfiguration configuration,
            LdapAuthenticationService ldapAuthenticationService,
            IUserAuthenticationService userAuthService, // Inject user auth service
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _ldapService = ldapAuthenticationService;
            _userAuthService = userAuthService; // Assign to private field
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
                return RedirectToAction("Index", "Messages");
            }

            ViewData["ReturnUrl"] = returnUrl;
            _logger.LogInformation("Displaying login page.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string loginType = "ldap", string returnUrl = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                AuthValidationResult validationResult;

                if (loginType.ToLower() == "ldap")
                {
                    var ldapResult = await _ldapService.ValidateUserAsync(username, password);
                    validationResult = new AuthValidationResult
                    {
                        IsValid = ldapResult.IsValid,
                        Roles = ldapResult.Roles,
                        ErrorMessage = ldapResult.ErrorMessage
                    };
                }
                else if (loginType.ToLower() == "web")
                {
                    validationResult = await _userAuthService.ValidateUserAsync(username, password);
                }
                else
                {
                    throw new InvalidOperationException("Invalid login type specified.");
                }

                if (validationResult.IsValid)
                {
                    _logger.LogInformation("User {Username} successfully authenticated via {LoginType}.", username, loginType);

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
                    _logger.LogInformation("User {Username} signed in successfully via {LoginType}.", username, loginType);

                    return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                        ? Redirect(returnUrl)
                        : RedirectToAction("Index", "Messages");
                }
                else
                {
                    _logger.LogWarning("Failed login attempt for user {Username} via {LoginType}. Error: {ErrorMessage}", username, loginType, validationResult.ErrorMessage);
                    ViewData["Message"] = validationResult.ErrorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to log in user {Username} via {LoginType}.", username, loginType);
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
