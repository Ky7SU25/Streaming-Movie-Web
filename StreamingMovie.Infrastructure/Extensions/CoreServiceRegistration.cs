using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.UnitOfWorks;
using StreamingMovie.Infrastructure.Extensions.Database;
using StreamingMovie.Infrastructure.Extensions.Mail;
using StreamingMovie.Infrastructure.UnitOfWork;
using System.Reflection;

namespace StreamingMovie.Infrastructure.Extensions;

/// <summary>
/// CoreServiceRegistration
/// </summary>
public static class CoreServiceRegistration
{
    public static IServiceCollection AddCoreInfrastructure(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddDatabase(config);
        services.AddMailService(config);

        services.AddScoped<IUnitOfWork, MySQLUnitOfWork>();

        services.AddScopedServicesByConvention(typeof(IMovieService).Assembly);

        services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

        return services;
    }

    public static void AddScopedServicesByConvention(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes();

        var interfaces = types
            .Where(t => t.IsInterface && t.Name.StartsWith("I"))
            .ToList();

        foreach (var iface in interfaces)
        {
            var impl = types.FirstOrDefault(c =>
                c.IsClass && !c.IsAbstract && iface.IsAssignableFrom(c));

            if (impl != null)
            {
                services.AddScoped(iface, impl);
            }
        }
    }
}
