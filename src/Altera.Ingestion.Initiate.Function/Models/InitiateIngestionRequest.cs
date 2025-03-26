namespace Altera.Ingestion.Initiate.Function.Models;

public record InitiateIngestionRequest
{
    public string OrganizationId { get; init; }

    public string EnterpriseId { get; init; }

    public string[] Files { get; init; }
}
