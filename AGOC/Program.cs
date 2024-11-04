using AGOC.Middleware;
using AGOC.Services;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add Serilog to the logging pipeline
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.ConfigureSqlServerContext(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddControllersWithViews();

try
{
    Log.Information("Starting up");

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<GeneralMiddleware>();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
