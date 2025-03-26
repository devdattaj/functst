using Altera.Ingestion.Status.Function.Models;

namespace Altera.Ingestion.Status.Function.Interfaces;

public interface IStatusService
{
    Task<JobStatusResponse> GetStatusAsync(string jobId, CancellationToken cancellationToken);
}
