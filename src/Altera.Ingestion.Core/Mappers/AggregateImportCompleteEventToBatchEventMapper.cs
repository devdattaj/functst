using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;

namespace Altera.Ingestion.Core.Mappers;

public class AggregateImportCompleteEventToBatchEventMapper : IMapper<AggregateImportCompleteEvent, BatchEvent>
{
    public BatchEvent Map(AggregateImportCompleteEvent source) => new BatchEvent
    {
        JobId = source.JobId,
        BatchId = source.BatchId,
        Errors = source.Errors,
        Stage = source.Type
    };
}
