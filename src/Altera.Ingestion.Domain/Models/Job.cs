namespace Altera.Ingestion.Domain.Models;

public record Job
{
    public string Id { get; init; }

    public JobStatus Status { get; init; }

    public string[] RawFiles { get; init; }
}
