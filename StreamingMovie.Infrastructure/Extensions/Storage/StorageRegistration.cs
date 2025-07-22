using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Application.Interfaces.ExternalServices.Storage;
using StreamingMovie.Infrastructure.ExternalServices.Storage;

namespace StreamingMovie.Infrastructure.Extensions.Storage;

public static class StorageRegistration
{
    public static IServiceCollection AddStorageService(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.Configure<MinioOptions>(config.GetSection("Storage"));

        services.AddSingleton<IStorageHandler, MinioStorageHandler>();
        return services;
    }
}
