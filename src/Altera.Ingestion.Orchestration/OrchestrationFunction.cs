using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Altera.Ingestion.Orchestration.Function.Interfaces;
using Altera.Ingestion.Integration.Messages.Events;
using Newtonsoft.Json.Linq;

namespace Altera.Ingestion.Orchestration.Function;

public class OrchestrationFunction
{
    private readonly ILogger<OrchestrationFunction> _logger;
    private readonly IStrategyFactory _strategyFactory;

    public OrchestrationFunction(
        ILogger<OrchestrationFunction> logger,
        IStrategyFactory strategyFactory)
    {
        _logger = logger;
        _strategyFactory = strategyFactory;
    }

    [Function("Orchestration")]
    public async Task RunAsync(
        [ServiceBusTrigger(
            topicName: "%ServiceBusTrigger:ReadTopicName%",
            subscriptionName: "%ServiceBusTrigger:SubscriptionName%",
            Connection = "ServiceBusTrigger"
            )]
        string  requestBody,
        FunctionContext context,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Orchestration function started");

        var eventType = JObject.Parse(requestBody);
        if (!eventType.TryGetValue(nameof(IServiceBusEvent.Type), out var typePropertyToken))
        {
            throw new ArgumentException($"No {nameof(IServiceBusEvent.Type)} was provided for the event.");
        }

        _logger.LogInformation("Processing event {EventType}", typePropertyToken.Value<string>());

        var strategy = _strategyFactory.GetStrategy(typePropertyToken.Value<string>());

        await strategy.ProcessEventAsync(requestBody, cancellationToken);

        _logger.LogInformation("Orchestration function completed");
    }
}
