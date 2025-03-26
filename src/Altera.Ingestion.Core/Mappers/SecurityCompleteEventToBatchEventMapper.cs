using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;

namespace Altera.Ingestion.Core.Mappers;

public class SecurityCompleteEventToBatchEventMapper : IMapper<SecurityCompleteEvent, BatchEvent>
{
    public BatchEvent Map(SecurityCompleteEvent source) => new BatchEvent
    {
        JobId = source.JobId,
        BatchId = source.BatchId,
        Errors = source.Errors,
        Stage = source.Type
    };
}
