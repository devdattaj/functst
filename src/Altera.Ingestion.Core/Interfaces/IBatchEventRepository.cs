using Altera.Ingestion.Core.Models.CosmosDb;

namespace Altera.Ingestion.Core.Interfaces;

public interface IBatchEventRepository
{
    Task<BatchEventData> CreateAsync(BatchEventData batchEvent, CancellationToken cancellationToken);
}
