namespace Altera.Ingestion.Load.Function.Options;

public record LandingStorageOptions
{
    public const string ConfigurationSectionName = "LandingStorage";

    public string LandingBlobStorageUri { get; init; }

    public string LandingBlobContainerName { get; init; }
}
