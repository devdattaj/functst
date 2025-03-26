namespace Altera.Ingestion.Core.Models.CosmosDb;

public record JobData
{
    public string Id { get; init; }

    public JobStatusData Status { get; init; }

    public int TotalBatches { get; init; }

    public int CompletedBatches { get; init; }

    public int FailedBatches { get; init; }

    public string[] RawFiles { get; init; }
}
