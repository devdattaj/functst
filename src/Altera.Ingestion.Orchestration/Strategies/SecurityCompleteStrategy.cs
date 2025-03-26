using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;
using Altera.Ingestion.Orchestration.Function.Interfaces;
using Newtonsoft.Json;

namespace Altera.Ingestion.Orchestration.Function.Strategies;

public class SecurityCompleteStrategy : IOrchestrationStrategy
{
    private readonly IOrchestrationService _orchestrationService;
    private readonly IMapperService _mapperService;

    public SecurityCompleteStrategy(IOrchestrationService orchestrationService, IMapperService mapperService)
    {
        _orchestrationService = orchestrationService;
        _mapperService = mapperService;
    }

    public async Task ProcessEventAsync(string json, CancellationToken cancellationToken)
    {
        var securityCompleteEvent = JsonConvert.DeserializeObject<SecurityCompleteEvent>(json);
        var batchEvent = _mapperService.Map<SecurityCompleteEvent, BatchEvent>(securityCompleteEvent);

        await _orchestrationService.SaveBatchEventAsync(batchEvent, cancellationToken);
    }
}
