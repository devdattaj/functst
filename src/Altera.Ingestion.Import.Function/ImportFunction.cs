using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Altera.Ingestion.Integration.Messages;
using Altera.Ingestion.Import.Function.Interfaces;

namespace Altera.Ingestion.Import.Function;

public class ImportFunction
{
    private readonly ILogger<ImportFunction> _logger;
    private readonly IImportService _importService;

    public ImportFunction(
        ILogger<ImportFunction> logger,
        IImportService importService)
    {
        _logger = logger;
        _importService = importService;
    }

    [Function("Import")]
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
        _logger.LogInformation("Import function started");

        var initiateImportRequest = JsonConvert.DeserializeObject<InitiateImportMessage>(requestBody);

        await _importService.ImportFilesAsync(initiateImportRequest, cancellationToken);

        _logger.LogInformation("Import function completed");
    }
}
