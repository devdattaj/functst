using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;
using Altera.Ingestion.Orchestration.Function.Interfaces;
using Newtonsoft.Json;

namespace Altera.Ingestion.Orchestration.Function.Strategies;

public class ValidationCompleteStrategy : IOrchestrationStrategy
{
    private readonly IOrchestrationService _orchestrationService;
    private readonly IMapperService _mapperService;

    public ValidationCompleteStrategy(IOrchestrationService orchestrationService, IMapperService mapperService)
    {
        _orchestrationService = orchestrationService;
        _mapperService = mapperService;
    }

    public async Task ProcessEventAsync(string json, CancellationToken cancellationToken)
    {
        var validationCompleteEvent = JsonConvert.DeserializeObject<ValidationCompleteEvent>(json);
        var batchEvent = _mapperService.Map<ValidationCompleteEvent, BatchEvent>(validationCompleteEvent);

        await _orchestrationService.SaveBatchEventAsync(batchEvent, cancellationToken);
    }
}
