namespace Altera.Ingestion.Core.Models.FhirService;

public record ImportStatusResponse
{
    public ImportStatusResponseOutput[] Output { get; init; }

    public object[] Error { get; init; }
}
