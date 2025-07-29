using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Extensions.Database;

/// <summary>
/// DatabseServiceRegistration
/// </summary>
public static class DatabseServiceRegistration
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var connectionString =
            config.GetConnectionString("sqlConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found."
            );

        services.AddDbContext<MovieDbContext>(options => options.UseMySQL(connectionString));
        // Cấu hình Identity
        services
            .AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;

                // Lockout configuration
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Locked for 5 minutes
                options.Lockout.MaxFailedAccessAttempts = 5; // Lock after 5 failed attempts
                options.Lockout.AllowedForNewUsers = true;

                // User configuration
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                // SignIn configuration
                options.SignIn.RequireConfirmedEmail = true; // Email must exist
                options.SignIn.RequireConfirmedPhoneNumber = false; //
            })
            .AddEntityFrameworkStores<MovieDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddAuthentication()
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddGoogle(options =>
            {
                options.ClientId = config["Authentication:Google:ClientId"];
                options.ClientSecret = config["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google";
                // Explicitly request the profile scope
                options.Scope.Add("profile");

                // Map the picture to a claim
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            });
        return services;
    }
}
