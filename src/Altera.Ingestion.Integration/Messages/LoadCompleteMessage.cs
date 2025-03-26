namespace Altera.Ingestion.Integration.Messages;

public record LoadCompleteMessage : IServiceBusMessage
{
    public string JobId { get; init; }

    public IEnumerable<FileMetadata> Files { get; init; }
}
