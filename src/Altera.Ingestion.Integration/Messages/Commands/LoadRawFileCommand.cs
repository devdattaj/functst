namespace Altera.Ingestion.Integration.Messages.Commands;

public record LoadRawFileCommand : IServiceBusMessage
{
    public string JobId { get; init; }

    public string RawFilePath { get; init; }
}
