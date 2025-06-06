using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;
using StreamingMovie.Infrastructure.Data;
using StreamingMovie.Infrastructure.Extensions.Database;
using StreamingMovie.Infrastructure.Extensions.Mail;
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

        services.AddScoped<IUnitOfWork, MySQLUnitOfWork>();

        return services;
    }
}
