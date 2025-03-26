using JobStatus = Altera.Ingestion.Domain.Models.JobStatus;

namespace Altera.Ingestion.Status.Function.Models;

public record JobStatusResponse
{
    public string JobId { get; init; }

    public JobStatus Status { get; init; }

    public string[] Files { get; init; }
}
