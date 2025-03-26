using Altera.Ingestion.Core.Interfaces;
using Microsoft.Azure.Functions.Worker;

namespace Altera.Ingestion.Core.Services;

public class CorrelationIdProvider : ICorrelationIdProvider
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    private const string CorrelationIdProperty = "CorrelationId";

    public string CorrelationId { get; private set; } = Guid.NewGuid().ToString();

    public async Task SetAsync(FunctionContext context)
    {
        var triggerType = context.FunctionDefinition.InputBindings.Values.First(x => x.Type.EndsWith("Trigger")).Type;

        switch (triggerType)
        {
            case "httpTrigger":
                var requestData = await context.GetHttpRequestDataAsync();
                var correlationIdHttp = requestData.Headers.TryGetValues(CorrelationIdHeader, out var values)
                    ? values.First()
                    : null;
                if (correlationIdHttp is not null)
                {
                    CorrelationId = correlationIdHttp;
                }
                break;
            case "serviceBusTrigger":
                context.BindingContext.BindingData.TryGetValue(CorrelationIdProperty, out var correlationIdSB);
                if (correlationIdSB is not null)
                {
                    CorrelationId = correlationIdSB.ToString();
                }
                break;
            default:
                throw new InvalidOperationException($"Unexpected trigger type '{triggerType}'.");
        };
    }
}
