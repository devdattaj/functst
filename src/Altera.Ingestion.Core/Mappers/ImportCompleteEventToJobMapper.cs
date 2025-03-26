using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;

namespace Altera.Ingestion.Core.Mappers;

public class ImportCompleteEventToJobMapper : IMapper<ImportCompleteEvent, Job>
{
    public Job Map(ImportCompleteEvent source) => new Job
    {
        Id = source.JobId,
        Status = JobStatus.Succeeded
    };
}
