using Altera.Ingestion.Core;
using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Initiate.Function.Interfaces;
using Altera.Ingestion.Initiate.Function.Models;
using Altera.Ingestion.Integration.Messages.Commands;
using Altera.Ingestion.Integration.Messages.Events;
using Microsoft.Extensions.Logging;

namespace Altera.Ingestion.Initiate.Function.Services;

public class InitiateIngestionService : IInitiateIngestionService
{
    private readonly ILogger<InitiateIngestionService> _logger;
    private readonly IServiceBusSenderService _serviceBusSenderService;

    public InitiateIngestionService(
        ILogger<InitiateIngestionService> logger,
        IServiceBusSenderService serviceBusSenderService)
    {
        _logger = logger;
        _serviceBusSenderService = serviceBusSenderService;
    }

    public async Task<string> InitiateIngestionAsync(InitiateIngestionRequest initiateIngestionRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(initiateIngestionRequest);

        var jobId = Guid.NewGuid().ToString();

        _logger.LogInformation("Sending initiate even with JobId '{JobId}'", jobId);

        var initiateEvent = new IngestionInitiatedEvent
        {
            JobId = jobId,
            RawFilePaths = initiateIngestionRequest.Files
        };

        await _serviceBusSenderService.SendMessageAsync(initiateEvent, ServiceBusTopic.Orchestration, cancellationToken);

        foreach (var file in initiateIngestionRequest.Files)
        {
            var loadRawFileCommand = new LoadRawFileCommand
            {
                JobId = jobId,
                RawFilePath = file
            };

            await _serviceBusSenderService.SendMessageAsync(loadRawFileCommand, ServiceBusTopic.Load, cancellationToken);
        }

        return jobId;
    }
}
