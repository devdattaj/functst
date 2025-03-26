namespace Altera.Ingestion.Integration.Messages;

public record InitiateImportMessage : IServiceBusMessage
{
    public string JobId { get; init; }

    public InitiateImportMessageFile[] Files { get; init; }
}
