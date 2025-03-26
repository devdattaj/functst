using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;
using Altera.Ingestion.Orchestration.Function.Interfaces;
using Newtonsoft.Json;

namespace Altera.Ingestion.Orchestration.Function.Strategies;

public class IngestionInitiatedStrategy : IOrchestrationStrategy
{
    private readonly IOrchestrationService _orchestrationService;
    private readonly IMapperService _mapperService;

    public IngestionInitiatedStrategy(IOrchestrationService orchestrationService, IMapperService mapperService)
    {
        _orchestrationService = orchestrationService;
        _mapperService = mapperService;
    }

    public async Task ProcessEventAsync(string json, CancellationToken cancellationToken)
    {
        var ingestionInitiatedEvent = JsonConvert.DeserializeObject<IngestionInitiatedEvent>(json);
        var job = _mapperService.Map<IngestionInitiatedEvent, Job>(ingestionInitiatedEvent);

        await _orchestrationService.SaveJobAsync(job, cancellationToken);
    }
}
