namespace Altera.Ingestion.Domain.Models;

public record BatchEvent
{
    public string BatchId { get; init; }

    public string JobId { get; init; }

    public string[] Errors { get; init; }

    public string Stage { get; init; }
}
