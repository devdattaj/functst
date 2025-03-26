using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Altera.Ingestion.Core.Middleware;

public class ServiceBusExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ServiceBusExceptionHandlerMiddleware> _logger;

    public ServiceBusExceptionHandlerMiddleware(ILogger<ServiceBusExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");

            throw;
        }
    }
}
