using Microsoft.AspNetCore.Authentication.Cookies;
using RentalTrack.Backoffice.Services;
using RentalTrack.Business.Interfaces;
using RentalTrack.Business.Services;
using RentalTrack.Data.Configuration;
using RentalTrack.Data.Connection;
using RentalTrack.Data.Dao;
using RentalTrack.Utility.Security;

namespace RentalTrack.Backoffice.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackofficeOptions(this IServiceCollection services, IConfiguration cfg)
    {
        services.Configure<DatabaseOptions>(cfg.GetSection(DatabaseOptions.SectionName));
        return services;
    }

    public static IServiceCollection AddBackofficeData(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<AdminUserDao>();
        services.AddScoped<UserDao>();
        services.AddScoped<PropertyDao>();
        services.AddScoped<DashboardDao>();
        return services;
    }

    public static IServiceCollection AddBackofficeUtility(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        return services;
    }

    public static IServiceCollection AddBackofficeBusiness(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IDashboardService, DashboardService>();
        return services;
    }

    public static IServiceCollection AddBackofficeServices(this IServiceCollection services)
    {
        services.AddSingleton<IEventLogService, InMemoryEventLogService>();
        return services;
    }

    public static IServiceCollection AddBackofficeAuth(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt =>
            {
                opt.Cookie.Name      = "RentalTrack.Backoffice.Auth";
                opt.LoginPath        = "/Account/Login";
                opt.LogoutPath       = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/Login";
                opt.ExpireTimeSpan   = TimeSpan.FromHours(8);
                opt.SlidingExpiration = true;
            });
        services.AddAuthorization();
        return services;
    }
}
