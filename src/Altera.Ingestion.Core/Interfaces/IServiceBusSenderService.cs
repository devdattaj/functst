using Altera.Ingestion.Integration.Messages;

namespace Altera.Ingestion.Core.Interfaces;

public interface IServiceBusSenderService
{
    Task SendMessageAsync<T>(T message, ServiceBusTopic topic, CancellationToken cancellationToken) where T : IServiceBusMessage;
}
