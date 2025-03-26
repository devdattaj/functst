using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Altera.Ingestion.Integration.Messages.Events;
using Altera.Ingestion.Load.Function.Interfaces;

namespace Altera.Ingestion.Load.Function;

public class LoadFunction
{
    private readonly ILogger<LoadFunction> _logger;
    private readonly ILoadService _loadService;

    public LoadFunction(
        ILogger<LoadFunction> logger,
        ILoadService loadService)
    {
        _logger = logger;
        _loadService = loadService;
    }


    [Function("Load")]
    public async Task RunAsync(
        [ServiceBusTrigger(
            topicName: "%ServiceBusTrigger:ReadTopicName%",
            subscriptionName: "%ServiceBusTrigger:SubscriptionName%",
            Connection = "ServiceBusTrigger"
            )]
        string requestBody,
        FunctionContext context,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Load function started");

        var initiateLoadRequest = JsonConvert.DeserializeObject<IngestionInitiatedEvent>(requestBody);

        await _loadService.LoadFilesAsync(initiateLoadRequest.RawFilePaths, initiateLoadRequest.JobId, cancellationToken);

        _logger.LogInformation("Load function completed");
    }
}
