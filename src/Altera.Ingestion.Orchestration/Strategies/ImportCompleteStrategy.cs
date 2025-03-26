using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Domain.Models;
using Altera.Ingestion.Integration.Messages.Events;
using Altera.Ingestion.Orchestration.Function.Interfaces;
using Newtonsoft.Json;

namespace Altera.Ingestion.Orchestration.Function.Strategies;

public class ImportCompleteStrategy : IOrchestrationStrategy
{
    private readonly IOrchestrationService _orchestrationService;
    private readonly IMapperService _mapperService;

    public ImportCompleteStrategy(IOrchestrationService orchestrationService, IMapperService mapperService)
    {
        _orchestrationService = orchestrationService;
        _mapperService = mapperService;
    }

    public async Task ProcessEventAsync(string json, CancellationToken cancellationToken)
    {
        var importCompleteEvent = JsonConvert.DeserializeObject<ImportCompleteEvent>(json);
        var job = _mapperService.Map<ImportCompleteEvent, Job>(importCompleteEvent);

        await _orchestrationService.UpdateJobAsync(job, cancellationToken);
    }
}
