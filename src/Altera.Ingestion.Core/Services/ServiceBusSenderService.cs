using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Integration.Messages;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Altera.Ingestion.Core.Services;

public class ServiceBusSenderService : IServiceBusSenderService
{
    private readonly ILogger<ServiceBusSenderService> _logger;
    private readonly IAzureClientFactory<ServiceBusSender> _senderFactory;
    private readonly ICorrelationIdProvider _correlationIdProvider;

    public ServiceBusSenderService(
        ILogger<ServiceBusSenderService> logger,
        IAzureClientFactory<ServiceBusSender> senderFactory,
        ICorrelationIdProvider correlationIdProvider)
    {
        _logger = logger;
        _correlationIdProvider = correlationIdProvider;
        _senderFactory = senderFactory;
    }

    public async Task SendMessageAsync<T>(T message, ServiceBusTopic topic, CancellationToken cancellationToken) where T : IServiceBusMessage
    {
        var messageSerialized = JsonConvert.SerializeObject(message);

        var serviceBusMessage = new ServiceBusMessage(messageSerialized)
        {
            CorrelationId = _correlationIdProvider.CorrelationId
        };

        _logger.LogDebug("Creating Service Bus Sender client for topic {TopicKey}", topic);

        var serviceBusSender = _senderFactory.CreateClient(topic);

        _logger.LogDebug("Sending service bus message {@ServiceBusMessage}", serviceBusMessage);

        await serviceBusSender.SendMessageAsync(serviceBusMessage, cancellationToken);
    }
}
