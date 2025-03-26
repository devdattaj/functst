using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Altera.Ingestion.Status.Function.Interfaces;
using Altera.Ingestion.Status.Function.Models;

namespace Altera.Ingestion.Status.Function.Services;

public class StatusService : IStatusService
{
    private readonly IJobRepository _jobRepository;
    private readonly IMapperService _mapperService;

    public StatusService(IJobRepository jobRepository, IMapperService mapperService)
    {
        _jobRepository = jobRepository;
        _mapperService = mapperService;
    }

    public async Task<JobStatusResponse> GetStatusAsync(string jobId, CancellationToken cancellationToken)
    {
        var response = await _jobRepository.GetAsync(jobId, cancellationToken);

        var result = _mapperService.Map<JobData, JobStatusResponse>(response);

        return result;
    }
}
