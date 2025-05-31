using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Application.Interfaces.ExternalServices.Mail;
using StreamingMovie.Infrastructure.ExternalServices.Mail;

namespace StreamingMovie.Infrastructure.Extensions.Mail;

public static class MailServiceRegistration
{
    public static IServiceCollection AddMailService(this IServiceCollection services, IConfiguration config)
    {
        var mailConfigs = config.GetSection("MailSettings");
        if (mailConfigs == null)
        {
            throw new InvalidOperationException("MailSettings configuration section not found.");
        }

        services.Configure<MailSettings>(mailConfigs);

        services.AddTransient<IMailService, MailService>();
        return services;
    }
}
