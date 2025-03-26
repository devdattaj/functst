using Altera.Ingestion.Core.Models.CosmosDb;

namespace Altera.Ingestion.Core.Interfaces;

public interface IJobRepository
{
    Task<JobData> CreateAsync(JobData job, CancellationToken cancellationToken);

    Task<JobData> GetAsync(string jobId, CancellationToken cancellationToken);

    Task<JobData> UpdateAsync(JobData job, CancellationToken cancellationToken);

    Task<JobData> IncrementTotalBatchesCountAsync(string jobId, CancellationToken cancellationToken);

    Task<JobData> IncrementCompletedBatchesCountAsync(string jobId, CancellationToken cancellationToken);
}
