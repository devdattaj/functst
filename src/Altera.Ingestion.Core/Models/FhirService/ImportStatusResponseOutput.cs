namespace Altera.Ingestion.Core.Models.FhirService;

public record ImportStatusResponseOutput
{
    public int Count { get; init; }

    public string InputUrl { get; init; }
}
