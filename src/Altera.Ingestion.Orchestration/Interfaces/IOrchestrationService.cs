using Altera.Ingestion.Domain.Models;

namespace Altera.Ingestion.Orchestration.Function.Interfaces;

public interface IOrchestrationService
{
    Task SaveJobAsync(Job job, CancellationToken cancellationToken);

    Task UpdateJobAsync(Job job, CancellationToken cancellationToken);

    Task SaveBatchAsync(Batch batch, CancellationToken cancellationToken);

    Task SaveBatchEventAsync(BatchEvent batchEvent, CancellationToken cancellationToken);

    Task CompleteBatchAsync(string jobId, CancellationToken cancellationToken);
}
