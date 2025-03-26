namespace Altera.Ingestion.Core.Options;

public record CosmosDbOptions
{
    public const string ConfigurationSectionName = "CosmosDb";

    public string Endpoint { get; init; }

    public string DatabaseId { get; init; }

    public string JobsContainerId { get; init; }

    public string BatchesContainerId { get; init; }

    public string BatchEventsContainerId { get; init; }
}
