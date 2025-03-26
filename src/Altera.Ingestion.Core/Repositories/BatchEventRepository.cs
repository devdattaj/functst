using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace Altera.Ingestion.Core.Repositories;

public class BatchEventRepository : IBatchEventRepository
{
    private readonly Container _batchEventsContainer;

    public BatchEventRepository(Container batchEventsContainer)
    {
        _batchEventsContainer = batchEventsContainer;
    }

    public async Task<BatchEventData> CreateAsync(BatchEventData batchEvent, CancellationToken cancellationToken)
    {
        var response = await _batchEventsContainer.CreateItemAsync(batchEvent, cancellationToken: cancellationToken);

        return response.Resource;
    }
}
