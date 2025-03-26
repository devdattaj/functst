namespace Altera.Ingestion.Core.Models.CosmosDb;

public record BatchData
{
    public string Id { get; init; }

    public string JobId { get; init; }

    public string FileName { get; init; }

    public string RawFileName { get; init; }
}
