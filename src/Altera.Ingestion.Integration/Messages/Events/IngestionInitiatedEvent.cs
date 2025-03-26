namespace Altera.Ingestion.Integration.Messages.Events;

public record IngestionInitiatedEvent : IServiceBusEvent
{
    public string JobId { get; init; }

    public string[] RawFilePaths { get; init; }

    public string Type => nameof(IngestionInitiatedEvent);
}
