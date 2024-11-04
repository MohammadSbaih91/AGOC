using AGOC.Domain;
using AGOC.Domain.Interfaces;
using AGOC.Domain.Manager;
using AGOC.Domain.Managers;
using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Repository.Repos;
using AGOC.Services.Interface;
using AGOC.Services.Managers;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AGOC.Services
{
    public static class ServiceExtention
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        //public static void ConfigureSqlServerContext(this IServiceCollection services, IConfiguration config)
        //{
        //    var connectionString = config.GetConnectionString("DefaultConnection");
        //    services.AddDbContext<VehicleMsContext>(options =>
        //        options.UseSqlServer(connectionString));
        //}
        public static void ConfigureSqlServerContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("AGOCDatabase");  // Use the new AGOC connection string
            services.AddDbContext<VehicleMsContext>(options =>
                options.UseSqlServer(connectionString));
        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.LoginPath = "/Account/Login";
                     options.AccessDeniedPath = "/Account/AccessDenied";
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                     options.SlidingExpiration = true;
                 });

            services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(@"C:\keys")) // or a network share, or other storage
            .SetApplicationName("AGOC");
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // Add AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register managers as scoped dependencies
            services.AddScoped<IVehicleManager, VehicleManager>();
            services.AddScoped<IParkingManager, ParkingManager>();
            services.AddScoped<ITrafficViolationManager, TrafficViolationManager>();
            services.AddScoped<IVehicleHandoverManager, VehicleHandoverManager>();
            services.AddScoped<IVehicleStatusManager, VehicleStatusManager>();
            services.AddScoped<ILookupViolationTypeManager, LookupViolationTypeManager>();
            services.AddScoped<ILookupVehicleStatusManager, LookupVehicleStatusManager>();
            services.AddScoped<IVehiclesLookupDetaileManager, VehiclesLookupDetaileManager>();
            services.AddScoped<IVehiclesLookupMainManager, VehiclesLookupMainManager>();
            services.AddScoped<IVehicleCategoryLookupManager, VehicleCategoryLookupManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsersManager, UsersManager>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddTransient<LdapAuthenticationService>();
            services.AddTransient<HrService>();
            services.AddScoped<ISmslogManager, SmslogManager>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddTransient<HelpersClass>();
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddScoped<IMessagesManager, MessagesManager>();


        }
    }
}