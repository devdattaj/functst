using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Altera.Ingestion.Status.Function.Interfaces;
using System.Web;
using Altera.Ingestion.Status.Function.Models;

namespace Altera.Ingestion.Status.Function;

public class StatusFunction
{
    private readonly ILogger<StatusFunction> _logger;
    private readonly IStatusService _initiateService;

    public StatusFunction(
        ILogger<StatusFunction> logger,
        IStatusService initiateService)
    {
        _logger = logger;
        _initiateService = initiateService;
    }

    [Function("Status")]
    public async Task<JobStatusResponse> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData request,
        FunctionContext context,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Status function started");

        var query = HttpUtility.ParseQueryString(request.Url.Query);
        var jobId = query["jobId"];

        var status = await _initiateService.GetStatusAsync(jobId, cancellationToken);

        _logger.LogInformation("Status function completed");

        return status;
    }
}
