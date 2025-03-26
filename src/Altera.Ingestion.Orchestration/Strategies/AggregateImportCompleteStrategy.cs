using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;
using Altera.Ingestion.Orchestration.Function.Interfaces;
using Newtonsoft.Json;

namespace Altera.Ingestion.Orchestration.Function.Strategies;

public class AggregateImportCompleteStrategy : IOrchestrationStrategy
{
    private readonly IOrchestrationService _orchestrationService;
    private readonly IMapperService _mapperService;

    public AggregateImportCompleteStrategy(IOrchestrationService orchestrationService, IMapperService mapperService)
    {
        _orchestrationService = orchestrationService;
        _mapperService = mapperService;
    }

    public async Task ProcessEventAsync(string json, CancellationToken cancellationToken)
    {
        var aggregateImportCompleteEvent = JsonConvert.DeserializeObject<AggregateImportCompleteEvent>(json);
        var batchEvent = _mapperService.Map<AggregateImportCompleteEvent, BatchEvent>(aggregateImportCompleteEvent);

        await _orchestrationService.SaveBatchEventAsync(batchEvent, cancellationToken);

        await _orchestrationService.CompleteBatchAsync(batchEvent.JobId, cancellationToken);
    }
}
