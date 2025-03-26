using Altera.Ingestion.Core.Models.CosmosDb;

namespace Altera.Ingestion.Core.Interfaces;

public interface IBatchRepository
{
    Task<BatchData> CreateAsync(BatchData batch, CancellationToken cancellationToken);

    Task<IList<string>> GetFileNamesByJobIdAsync(string jobId, CancellationToken cancellationToken);
}
