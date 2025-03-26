using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;

namespace Altera.Ingestion.Core.Mappers;

public class LoadCompleteEventToBatchMapper : IMapper<LoadCompleteEvent, Batch>
{
    public Batch Map(LoadCompleteEvent source) => new Batch
    {
        Id = source.BatchId,
        JobId = source.JobId,
        FileName = source.FileName,
        RawFileName = source.RawFileName
    };
}
