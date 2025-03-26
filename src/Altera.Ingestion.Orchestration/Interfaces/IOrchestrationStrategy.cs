
namespace Altera.Ingestion.Orchestration.Function.Interfaces;

public interface IOrchestrationStrategy
{
    Task ProcessEventAsync(string json, CancellationToken cancellationToken);
}
