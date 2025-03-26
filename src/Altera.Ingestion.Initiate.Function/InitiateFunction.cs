using Altera.Ingestion.Initiate.Function.Interfaces;
using Altera.Ingestion.Initiate.Function.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Altera.Ingestion.Initiate.Function;

public class InitiateFunction
{
    private readonly ILogger<InitiateFunction> _logger;
    private readonly IInitiateIngestionService _initiateIngestionService;

    public InitiateFunction(
        ILogger<InitiateFunction> logger,
        IInitiateIngestionService initiateIngestionService)
    {
        _logger = logger;
        _initiateIngestionService = initiateIngestionService;
    }

    [Function("Initiate")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
        FunctionContext context,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initiate function started");

        using var streamReader = new StreamReader(request.Body);
        var requestBody = await streamReader.ReadToEndAsync(cancellationToken);
        var initiateIngestionRequest = JsonConvert.DeserializeObject<InitiateIngestionRequest>(requestBody);

        _logger.LogDebug("{@InitiateRequest}", initiateIngestionRequest);

        var jobId = await _initiateIngestionService.InitiateIngestionAsync(initiateIngestionRequest, cancellationToken);

        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { jobId }, cancellationToken);

        _logger.LogInformation("Initiate function completed");

        return response;
    }
}
