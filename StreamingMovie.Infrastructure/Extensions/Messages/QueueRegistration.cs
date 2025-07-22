using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Application.Interfaces.ExternalServices.Messages;
using StreamingMovie.Infrastructure.ExternalServices.Messages;

namespace StreamingMovie.Infrastructure.Extensions.Messages;

public static class QueueRegistration
{
    public static IServiceCollection AddRabbitMq(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddSingleton<RabbitMqConnectionFactory>();
        services.AddSingleton<IQueuePublisher, RabbitMqPublisher>();
        services.Configure<RabbitMqOptions>(config.GetRequiredSection("RabbitMq"));
        return services;
    }
}
