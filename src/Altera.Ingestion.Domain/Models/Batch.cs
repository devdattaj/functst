namespace Altera.Ingestion.Domain.Models;

public record Batch
{
    public string Id { get; init; }

    public string JobId { get; init; }

    public string FileName { get; init; }

    public string RawFileName { get; init; }
}
