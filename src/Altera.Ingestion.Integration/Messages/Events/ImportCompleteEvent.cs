namespace Altera.Ingestion.Integration.Messages.Events;

public record ImportCompleteEvent : IServiceBusEvent
{
    public string JobId { get; init; }

    public string Type => nameof(ImportCompleteEvent);
}
