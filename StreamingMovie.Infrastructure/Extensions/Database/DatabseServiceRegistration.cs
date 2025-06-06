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
            })
            .AddEntityFrameworkStores<MovieDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}
