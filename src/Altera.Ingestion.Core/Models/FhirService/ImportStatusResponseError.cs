namespace Altera.Ingestion.Core.Models.FhirService;

public record ImportStatusResponseError
{
    public int Count { get; init; }

    public string InputUrl { get; init; }

    public string Url { get; init; }
}
