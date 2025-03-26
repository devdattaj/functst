using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace Altera.Ingestion.Core.Repositories;

public class JobRepository : IJobRepository
{
    private const string TotalBatchesPath = "/totalBatches";
    private const string CompletedBatchesPath = "/completedBatches";

    private readonly Container _jobsContainer;

    public JobRepository(Container jobsContainer)
    {
        _jobsContainer = jobsContainer;
    }

    public async Task<JobData> CreateAsync(JobData job, CancellationToken cancellationToken)
    {
        var response = await _jobsContainer.CreateItemAsync(job, cancellationToken: cancellationToken);

        return response.Resource;
    }

    public async Task<JobData> GetAsync(string jobId, CancellationToken cancellationToken)
    {
        var response = await _jobsContainer.ReadItemAsync<JobData>(jobId, new PartitionKey(jobId), null, cancellationToken);

        return response.Resource;
    }

    public async Task<JobData> UpdateAsync(JobData job, CancellationToken cancellationToken)
    {
        var response = await _jobsContainer.ReplaceItemAsync(job, job.Id, cancellationToken: cancellationToken);

        return response.Resource;
    }

    public async Task<JobData> IncrementTotalBatchesCountAsync(string jobId, CancellationToken cancellationToken)
    {
        return await PatchIncrementAsync(jobId, TotalBatchesPath, cancellationToken);
    }

    public async Task<JobData> IncrementCompletedBatchesCountAsync(string jobId, CancellationToken cancellationToken)
    {
        return await PatchIncrementAsync(jobId, CompletedBatchesPath, cancellationToken);
    }

    private async Task<JobData> PatchIncrementAsync(string jobId, string path, CancellationToken cancellationToken)
    {
        var patchOperations = new List<PatchOperation>
        {
            PatchOperation.Increment(path, 1)
        };

        var response = await _jobsContainer.PatchItemAsync<JobData>(jobId, partitionKey: new PartitionKey(jobId), patchOperations: patchOperations, cancellationToken: cancellationToken);

        return response;
    }
}
