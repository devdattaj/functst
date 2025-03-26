namespace Altera.Ingestion.Integration.Messages;

public record FileMetadata
{
    public string Uri { get; init; }

    public string Etag { get; init; }
}
