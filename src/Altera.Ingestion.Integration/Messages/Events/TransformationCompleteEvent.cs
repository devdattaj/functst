namespace Altera.Ingestion.Integration.Messages.Events;

public record TransformationCompleteEvent : IServiceBusEvent
{
    public string BatchId { get; init; }

    public string JobId { get; init; }

    public string[] Errors { get; init; }

    public string Type => nameof(TransformationCompleteEvent);
}
