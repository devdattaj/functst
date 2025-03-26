using Altera.Ingestion.Core.Middleware;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;

namespace Altera.Ingestion.Core;

public static class ApplicationBuilderExtensions
{
    public static void AddLogEnrichment(this IFunctionsWorkerApplicationBuilder builder)
    {
        builder.UseMiddleware<LogEnricherMiddleware>();
    }

    public static void AddHttpMiddleware(this IFunctionsWorkerApplicationBuilder builder)
    {
        builder.UseMiddleware<HttpExceptionHandlerMiddleware>();
    }

    public static void AddServiceBusMiddleware(this IFunctionsWorkerApplicationBuilder builder)
    {
        builder.UseMiddleware<ServiceBusExceptionHandlerMiddleware>();
    }
}
