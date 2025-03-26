using Microsoft.Azure.Functions.Worker;

namespace Altera.Ingestion.Core.Interfaces;

public interface ICorrelationIdProvider
{
    string CorrelationId { get; }

    Task SetAsync(FunctionContext context);
}
