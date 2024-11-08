using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AGOC.Middleware
{
    public class GeneralMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<GeneralMiddleware> _logger;

        public GeneralMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, ILogger<GeneralMiddleware> logger)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    _logger.LogInformation("GeneralMiddleware invoked for path: {Path}", context.Request.Path);

                    if (context.Request.QueryString.HasValue && context.Request.QueryString.Value.Length > 2048)
                    {
                        _logger.LogWarning("QueryString length exceeded 2048 characters. Redirecting to Login.");
                        context.Response.Redirect("/Account/Login");
                        return;
                    }

                    if (context.User.Identity.IsAuthenticated && context.Request.Path.StartsWithSegments("/Account/Login"))
                    {
                        _logger.LogInformation("User is already authenticated. Redirecting to AGOC/Index.");
                        context.Response.Redirect("/AGOC/Index");
                        return;
                    }

                    var authCookie = context.Request.Cookies[".AspNetCore.Cookies"];
                    if (!string.IsNullOrEmpty(authCookie))
                    {
                        var ticket = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        if (ticket.Succeeded)
                        {
                            var expireUtc = ticket.Properties.ExpiresUtc;
                            if (expireUtc.HasValue && expireUtc.Value.UtcDateTime < DateTime.UtcNow)
                            {
                                _logger.LogInformation("Authentication ticket has expired. Redirecting to Login.");
                                context.Response.Redirect($"/Account/Login?ReturnUrl={context.Request.Path}");
                                return;
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Authentication ticket was not successful.");
                        }
                    }

                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in GeneralMiddleware for path: {Path}", context.Request.Path);
                    throw;
                }
            }
        }
    }
}