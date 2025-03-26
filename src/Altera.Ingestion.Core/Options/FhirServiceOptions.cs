namespace Altera.Ingestion.Core.Options;

public record FhirServiceOptions
{
    public const string ConfigurationSectionName = "FhirService";

    public string ServiceBaseUri { get; init; }

    public string AuthBaseUri { get; init; }

    public string ClientId { get; init; }

    public string ClientSecret { get; init; }
}
