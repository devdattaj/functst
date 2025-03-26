namespace Altera.Ingestion.Integration.Messages.Events;

public record LoadCompleteEvent : IServiceBusEvent
{
    public string BatchId { get; init; }

    public string JobId { get; init; }

    public string FileName { get; init; }

    public string RawFileName { get; init; }

    public string Type => nameof(LoadCompleteEvent);
}
