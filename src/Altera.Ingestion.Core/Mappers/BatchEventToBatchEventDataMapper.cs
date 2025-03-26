using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Altera.Ingestion.Domain.Models;

namespace Altera.Ingestion.Core.Mappers;

public class BatchEventToBatchEventDataMapper : IMapper<BatchEvent, BatchEventData>
{
    public BatchEventData Map(BatchEvent source) => new BatchEventData
    {
        JobId = source.JobId,
        BatchId = source.BatchId,
        Errors = source.Errors,
        Stage = source.Stage
    };
}

