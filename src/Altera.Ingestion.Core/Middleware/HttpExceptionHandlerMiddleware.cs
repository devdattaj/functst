using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Altera.Ingestion.Core.Middleware;

public class HttpExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<HttpExceptionHandlerMiddleware> _logger;

    public HttpExceptionHandlerMiddleware(ILogger<HttpExceptionHandlerMiddleware> logger)
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

            var httpRequestData = await context.GetHttpRequestDataAsync();
            var response = httpRequestData.CreateResponse(HttpStatusCode.InternalServerError);

            var errorMessage = new { Message = "An unhandled exception occurred." };
            string responseBody = JsonConvert.SerializeObject(errorMessage);
            await response.WriteStringAsync(responseBody);

            context.GetInvocationResult().Value = response;
        }
    }
}
