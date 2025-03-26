using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;

namespace Altera.Ingestion.Core.Mappers;

public class IngestionInitiatedEventToJobMapper : IMapper<IngestionInitiatedEvent, Job>
{
    public Job Map(IngestionInitiatedEvent source) => new Job
    {
        Id = source.JobId,
        RawFiles = source.RawFilePaths,
        Status = JobStatus.Started
    };
}
