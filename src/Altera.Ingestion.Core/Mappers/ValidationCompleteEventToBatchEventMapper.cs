using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;

namespace Altera.Ingestion.Core.Mappers;

public class ValidationCompleteEventToBatchEventMapper : IMapper<ValidationCompleteEvent, BatchEvent>
{
    public BatchEvent Map(ValidationCompleteEvent source) => new BatchEvent
    {
        JobId = source.JobId,
        BatchId = source.BatchId,
        Errors = source.Errors,
        Stage = source.Type
    };
}
