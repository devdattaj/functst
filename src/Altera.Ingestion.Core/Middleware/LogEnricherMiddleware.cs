using Altera.Ingestion.Core.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Context;

namespace Altera.Ingestion.Core.Middleware;

public class LogEnricherMiddleware : IFunctionsWorkerMiddleware
{
    private const string CorrelationIdLogPropertyName = "CorrelationId";

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // Resolved via context because CorrelationIdProvider instance needs to match lifetime of the function
        var correlationIdProvider = context.InstanceServices.GetRequiredService<ICorrelationIdProvider>();
        await correlationIdProvider.SetAsync(context);

        using var _correlationid = LogContext.PushProperty(CorrelationIdLogPropertyName, correlationIdProvider.CorrelationId);

        await next(context);
    }
}
