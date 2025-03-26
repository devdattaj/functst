using Altera.Ingestion.Core;
using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.FhirService;
using Altera.Ingestion.Import.Function.Exceptions;
using Altera.Ingestion.Import.Function.Interfaces;
using Altera.Ingestion.Import.Function.Options;
using Altera.Ingestion.Integration.Messages;
using Altera.Ingestion.Integration.Messages.Events;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Task = System.Threading.Tasks.Task;

namespace Altera.Ingestion.Import.Function.Services;

public class ImportService : IImportService
{
    private readonly ImportOptions _options;
    private readonly IFhirApiService _fhirApiClient;
    private readonly IServiceBusSenderService _serviceBusSenderService;

    public ImportService(
        IOptions<ImportOptions> options,
        IFhirApiService fhirApiClient,
        IServiceBusSenderService serviceBusSenderService)
    {
        _options = options.Value;
        _fhirApiClient = fhirApiClient;
        _serviceBusSenderService = serviceBusSenderService;
    }

    public async Task ImportFilesAsync(InitiateImportMessage initiateImportMessage, CancellationToken cancellationToken)
    {
        var uri = await _fhirApiClient.InitiateImportOperationAsync(initiateImportMessage, cancellationToken);

        var importOperationId = int.Parse(uri.Segments.Last());

        var stopwatch = Stopwatch.StartNew();

        ImportStatusResponse importStatus = null;

        while (stopwatch.Elapsed < TimeSpan.FromSeconds(_options.ImportTimeoutSeconds))
        {
            importStatus = await _fhirApiClient.GetImportOperationStatusAsync(importOperationId, cancellationToken);

            if (importStatus != null)
            {
                break;
            }

            await Task.Delay(TimeSpan.FromSeconds(_options.ImportPollPeriodSeconds), cancellationToken);
        };

        if (importStatus == null)
        {
            throw new ImportCompletionNotReachedException($"The wait for $import operation with id '{importOperationId}' has timed out.");
        }

        var importEvent = new ImportCompleteEvent
        {
            JobId = initiateImportMessage.JobId
        };

        await _serviceBusSenderService.SendMessageAsync(importEvent, ServiceBusTopic.Orchestration, cancellationToken);
    }
}
