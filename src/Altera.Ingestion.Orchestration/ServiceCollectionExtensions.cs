using Altera.Ingestion.Orchestration.Function.Interfaces;
using Altera.Ingestion.Orchestration.Function.Services;
using Altera.Ingestion.Orchestration.Function.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace Altera.Ingestion.Orchestration.Function;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFunctionServices(this IServiceCollection services)
    {
        services.AddTransient<IOrchestrationService, OrchestrationService>();

        services.AddTransient<IStrategyFactory, StrategyFactory>();

        services.AddTransient<IngestionInitiatedStrategy>();
        services.AddTransient<LoadCompleteStrategy>();
        services.AddTransient<ValidationCompleteStrategy>();
        services.AddTransient<SecurityCompleteStrategy>();
        services.AddTransient<TransformationCompleteStrategy>();
        services.AddTransient<AggregateImportCompleteStrategy>();
        services.AddTransient<ImportCompleteStrategy>();

        return services;
    }
}
