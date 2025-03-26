using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace Altera.Ingestion.Core.Repositories;

public class BatchRepository : IBatchRepository
{
    private readonly Container _batchesContainer;
    private readonly ICosmosLinqQuery _cosmosLinqQuery;

    public BatchRepository(Container batchesContainer, ICosmosLinqQuery cosmosLinqQuery)
    {
        _batchesContainer = batchesContainer;
        _cosmosLinqQuery = cosmosLinqQuery;
    }

    public async Task<BatchData> CreateAsync(BatchData batch, CancellationToken cancellationToken)
    {
        var response = await _batchesContainer.CreateItemAsync(batch, cancellationToken: cancellationToken);

        return response.Resource;
    }

    public async Task<IList<string>> GetFileNamesByJobIdAsync(string jobId, CancellationToken cancellationToken)
    {
        var query = _batchesContainer
            .GetItemLinqQueryable<BatchData>()
            .Where(x => x.JobId == jobId);

        using var feed = _cosmosLinqQuery.GetFeedIterator(query);

        var fileNames = new List<string>();
        while (feed.HasMoreResults)
        {
            var result = await feed.ReadNextAsync(cancellationToken);

            fileNames.AddRange(result.Select(x => x.FileName));
        }

        return fileNames;
    }
}
