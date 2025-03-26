namespace Altera.Ingestion.Orchestration.Function.Interfaces;

public interface IOrchestrationProcessor
{
    void SetStrategy(IOrchestrationStrategy orchestrationStrategy);

    Task ProcessAsync(string eventJson, CancellationToken cancellationToken);
}
