using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Status.Function.Models;

namespace Altera.Ingestion.Status.Function.Mappers;

public class JobDataToJobStatusResponseMapper : IMapper<JobData, JobStatusResponse>
{
    public JobStatusResponse Map(JobData source) => new JobStatusResponse
    {
        JobId = source.Id,
        Status = source.Status switch
        {
            JobStatusData.Started => JobStatus.Started,
            JobStatusData.Succeeded => JobStatus.Succeeded,
            JobStatusData.Failed => JobStatus.Failed,
            _ => throw new Exception($"Unexcepted job status {source.Status}"),
        },
        Files = source.RawFiles
    };
}
