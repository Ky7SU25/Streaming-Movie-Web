using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.UnitOfWorks;
using StreamingMovie.Infrastructure.Extensions.Database;
using StreamingMovie.Infrastructure.Extensions.GoogleAuth;
using StreamingMovie.Infrastructure.Extensions.Huggingface;
using StreamingMovie.Infrastructure.Extensions.Mail;
using StreamingMovie.Infrastructure.Extensions.Messages;
using StreamingMovie.Infrastructure.Extensions.Payment;
using StreamingMovie.Infrastructure.Extensions.Storage;
using StreamingMovie.Infrastructure.UnitOfWork;

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
        services.AddGoogleAuthService(config);
        services.AddStorageService(config);
        services.AddRabbitMq(config);
        services.AddPayment(config);
        services.AddHuggingfaceService();


        services.AddScoped<IUnitOfWork, MySQLUnitOfWork>();

        services.AddScopedServicesByConvention(typeof(IMovieService).Assembly);

        services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

        return services;
    }

    public static void AddScopedServicesByConvention(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var loadedPaths = AppDomain.CurrentDomain.GetAssemblies();
        Type[] types = loadedPaths.SelectMany(a => a.GetTypes()).ToArray();

        var interfaces = types
            .Where(t =>
                t.IsInterface
                && t.Name.StartsWith("I")
                && (t.Name.EndsWith("Repository") || t.Name.EndsWith("Service"))
                && (t.Namespace?.StartsWith("StreamingMovie.") ?? false)
            )
            .ToList();

        foreach (var iface in interfaces)
        {
            var impl = types.FirstOrDefault(c =>
                c.IsClass && !c.IsAbstract && iface.IsAssignableFrom(c)
            );

            if (impl != null)
            {
                services.AddScoped(iface, impl);
            }
        }
    }
}
