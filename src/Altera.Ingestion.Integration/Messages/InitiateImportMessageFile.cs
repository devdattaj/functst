namespace Altera.Ingestion.Integration.Messages;

public record InitiateImportMessageFile
{
    public string Uri { get; init; }

    public string Etag { get; init; }
}
