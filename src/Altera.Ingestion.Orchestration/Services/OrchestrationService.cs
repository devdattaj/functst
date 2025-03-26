using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.CosmosDb;
using Altera.Ingestion.Integration.Messages;
using Altera.Ingestion.Orchestration.Function.Interfaces;
using Altera.Ingestion.Core;
using Altera.Ingestion.Domain.Models;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Altera.Ingestion.Orchestration.Function.Services;

public class OrchestrationService : IOrchestrationService
{
    private readonly ILogger<OrchestrationService> _logger;
    private readonly IMapperService _mapperService;
    private readonly IJobRepository _jobRepository;
    private readonly IBatchRepository _batchRepository;
    private readonly IBatchEventRepository _batchEventRepository;
    private readonly IServiceBusSenderService _serviceBusSenderService;

    public OrchestrationService(
        ILogger<OrchestrationService> logger,
        IMapperService mapperService,
        IJobRepository jobRepository,
        IBatchRepository batchRepository,
        IBatchEventRepository batchEventRepository,
        IServiceBusSenderService serviceBusSenderService)
    {
        _logger = logger;
        _mapperService = mapperService;
        _jobRepository = jobRepository;
        _batchRepository = batchRepository;
        _batchEventRepository = batchEventRepository;
        _serviceBusSenderService = serviceBusSenderService;
    }

    public async Task SaveJobAsync(Job job, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Saving job with id: {JobId}", job.Id);

        var jobData = _mapperService.Map<Job, JobData>(job);

        await _jobRepository.CreateAsync(jobData, cancellationToken);
    }

    public async Task UpdateJobAsync(Job job, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating job with id: {JobId}", job.Id);

        var jobData = _mapperService.Map<Job, JobData>(job);

        await _jobRepository.UpdateAsync(jobData, cancellationToken);
    }

    public async Task SaveBatchAsync(Batch batch, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Saving batch with id: {BatchId} and job id: {JobId}", batch.Id, batch.JobId);

        var batchData = _mapperService.Map<Batch, BatchData>(batch);
        var jobId = batch.JobId;

        await _batchRepository.CreateAsync(batchData, cancellationToken);
        var job = await _jobRepository.IncrementTotalBatchesCountAsync(jobId, cancellationToken);
        _logger.LogInformation("Total batches count: {TempTotalBatches} for job id: {JobId}", job.TotalBatches, job.Id);
    }

    public async Task SaveBatchEventAsync(BatchEvent batchEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Saving batch event with batch id: {BatchId} and job id: {JobId}", batchEvent.BatchId, batchEvent.JobId);
        var batchEventData = _mapperService.Map<BatchEvent, BatchEventData>(batchEvent);

        await _batchEventRepository.CreateAsync(batchEventData, cancellationToken);
    }

    public async Task CompleteBatchAsync(string jobId, CancellationToken cancellationToken)
    {
        using var _ = LogContext.PushProperty("JobId", jobId);
        _logger.LogInformation("Completing batch");

        var job = await _jobRepository.IncrementCompletedBatchesCountAsync(jobId, cancellationToken);

        if (job.TotalBatches == job.CompletedBatches + job.FailedBatches)
        {
            _logger.LogInformation("Number of total batches reached. Total: {TotalBatches} Completed: {CompletedBatches} Failed: {FailedBatches}",
                job.TotalBatches, job.CompletedBatches, job.FailedBatches);

            var fileNames = await _batchRepository.GetFileNamesByJobIdAsync(jobId, cancellationToken);
            _logger.LogDebug("Sending file names {FileNames}", fileNames);

            var message = new InitiateImportMessage
            {
                JobId = jobId,
                Files = fileNames.Select(x => new InitiateImportMessageFile { Uri = x }).ToArray()
            };

            await _serviceBusSenderService.SendMessageAsync(message, ServiceBusTopic.InitiateImport, cancellationToken);

            return;
        }

        _logger.LogInformation("The job is not complete after batch completion. Total: {TempTotalBatches} Completed: {TempCompletedBatches} Failed: {TempFailedBatches}",
            job.TotalBatches, job.CompletedBatches, job.FailedBatches);
    }
}
