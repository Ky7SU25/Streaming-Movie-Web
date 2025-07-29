using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Application.Interfaces.ExternalServices.Huggingface;
using StreamingMovie.Infrastructure.ExternalServices.Huggingface;

namespace StreamingMovie.Infrastructure.Extensions.Huggingface;

public static class HuggingfaceServiceRegistration
{
    public static IServiceCollection AddHuggingfaceService(
        this IServiceCollection services
    )
    {
        services.AddHttpClient<IEmbeddingService, EmbeddingService>();

        return services;
    }
}
