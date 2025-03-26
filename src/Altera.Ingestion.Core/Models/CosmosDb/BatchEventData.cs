namespace Altera.Ingestion.Core.Models.CosmosDb;

public record BatchEventData
{
    public string Id { get; } = Guid.NewGuid().ToString();

    public string BatchId { get; init; }

    public string JobId { get; init; }

    public string Stage { get; init; }

    public string[] Errors { get; init; }
}
