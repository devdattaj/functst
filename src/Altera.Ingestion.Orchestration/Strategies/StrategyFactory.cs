using Altera.Ingestion.Integration.Messages.Events;
using Altera.Ingestion.Orchestration.Function.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Altera.Ingestion.Orchestration.Function.Strategies;

public class StrategyFactory : IStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public StrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IOrchestrationStrategy GetStrategy(string type) => type switch
    {
        nameof(IngestionInitiatedEvent) => _serviceProvider.GetRequiredService<IngestionInitiatedStrategy>(),
        nameof(LoadCompleteEvent) => _serviceProvider.GetRequiredService<LoadCompleteStrategy>(),
        nameof(ValidationCompleteEvent) => _serviceProvider.GetRequiredService<ValidationCompleteStrategy>(),
        nameof(SecurityCompleteEvent) => _serviceProvider.GetRequiredService<SecurityCompleteStrategy>(),
        nameof(TransformationCompleteEvent) => _serviceProvider.GetRequiredService<TransformationCompleteStrategy>(),
        nameof(AggregateImportCompleteEvent) => _serviceProvider.GetRequiredService<AggregateImportCompleteStrategy>(),
        nameof(ImportCompleteEvent) => _serviceProvider.GetRequiredService<ImportCompleteStrategy>(),
        _ => throw new ArgumentException($"Unexpected event type '{type}'.")
    };
}
