using Altera.Ingestion.Core;
using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Integration.Messages;
using Altera.Ingestion.Integration.Messages.Events;
using Altera.Ingestion.Load.Function.Interfaces;

namespace Altera.Ingestion.Load.Function.Services;

public class LoadService : ILoadService
{
    private readonly ILandingRepository _landingRepository;
    private readonly IServiceBusSenderService _serviceBusSenderService;

    public LoadService(
        ILandingRepository landingRepository,
        IServiceBusSenderService serviceBusSenderService)
    {
        _landingRepository = landingRepository;
        _serviceBusSenderService = serviceBusSenderService;
    }

    public async Task LoadFilesAsync(string[] filePaths, string jobId, CancellationToken cancellationToken)
    {
        var filesMetadata = await _landingRepository.CopyFromAsync(filePaths, cancellationToken);

        var files = new LoadCompleteMessage
        {
            JobId = jobId,
            Files = filesMetadata
        };

        await _serviceBusSenderService.SendMessageAsync(files, ServiceBusTopic.Validate, cancellationToken);

        var loadEvent = new LoadCompleteEvent
        {
            BatchId = Guid.NewGuid().ToString(),
            JobId = jobId,
            // todo add filename and rawfilename
        };
        await _serviceBusSenderService.SendMessageAsync(loadEvent, ServiceBusTopic.Orchestration, cancellationToken);
    }
}
