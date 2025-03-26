using Altera.Ingestion.Initiate.Function.Interfaces;
using Altera.Ingestion.Initiate.Function.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Altera.Ingestion.Initiate.Function;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInitiateFunctionServices(this IServiceCollection services)
    {
        services.AddTransient<IInitiateIngestionService, InitiateIngestionService>();

        return services;
    }
}
