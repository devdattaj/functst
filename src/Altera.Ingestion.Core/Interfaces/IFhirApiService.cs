using Altera.Ingestion.Core.Models.FhirService;
using Altera.Ingestion.Integration.Messages;

namespace Altera.Ingestion.Core.Interfaces;

public interface IFhirApiService
{
    Task<Uri> InitiateImportOperationAsync(InitiateImportMessage initiateLoadMessage, CancellationToken cancellationToken);

    Task<ImportStatusResponse> GetImportOperationStatusAsync(int importOperationId, CancellationToken cancellationToken);
}
