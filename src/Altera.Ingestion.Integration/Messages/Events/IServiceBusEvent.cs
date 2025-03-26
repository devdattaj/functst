namespace Altera.Ingestion.Integration.Messages.Events;

public interface IServiceBusEvent : IServiceBusMessage
{
    string Type { get; }
}
