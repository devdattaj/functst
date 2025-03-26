namespace Altera.Ingestion.Integration.Messages;

public record ImportCompleteMessage : IServiceBusMessage
{
    public string JobId { get; init; }

    public string[] Files { get; init; }
}
