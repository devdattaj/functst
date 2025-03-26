using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;

namespace Altera.Ingestion.Core.Mappers;

public class TransformationCompleteEventToBatchEventMapper : IMapper<TransformationCompleteEvent, BatchEvent>
{
    public BatchEvent Map(TransformationCompleteEvent source) => new BatchEvent
    {
        JobId = source.JobId,
        BatchId = source.BatchId,
        Errors = source.Errors,
        Stage = source.Type
    };
}
