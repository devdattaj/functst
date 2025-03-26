using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Altera.Ingestion.Domain.Models;

namespace Altera.Ingestion.Core.Mappers;

public class JobToJobDataMapper : IMapper<Job, JobData>
{
    public JobData Map(Job source) => new JobData
    {
        Id = source.Id,
        Status = source.Status switch
        {
            JobStatus.Started => JobStatusData.Started,
            JobStatus.Succeeded => JobStatusData.Succeeded,
            JobStatus.Failed => JobStatusData.Failed,
            _ => throw new ArgumentException($"Unexpected source {nameof(JobStatus)} 'source.Status'."),
        },
        RawFiles = source.RawFiles
    };
}
