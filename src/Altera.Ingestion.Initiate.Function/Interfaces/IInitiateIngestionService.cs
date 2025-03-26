using Altera.Ingestion.Initiate.Function.Models;

namespace Altera.Ingestion.Initiate.Function.Interfaces;

public interface IInitiateIngestionService
{
    Task<string> InitiateIngestionAsync(InitiateIngestionRequest initiateIngestionRequest, CancellationToken cancellationToken);
}
