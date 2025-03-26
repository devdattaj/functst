using Altera.Ingestion.Integration.Messages;
using Altera.Ingestion.Load.Function.Interfaces;
using Altera.Ingestion.Load.Function.Options;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Altera.Ingestion.Load.Function.Repositories;

public class LandingRepository : ILandingRepository
{
    private readonly LandingStorageOptions _storageOptions;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<LandingRepository> _logger;

    public LandingRepository(IOptions<LandingStorageOptions> storageOptions, BlobServiceClient blobServiceClient, ILogger<LandingRepository> logger)
    {
        _storageOptions = storageOptions.Value;
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    public async Task<IEnumerable<FileMetadata>> CopyFromAsync(string[] filePaths, CancellationToken cancelationToken)
    {
        var operations = new List<CopyFromUriOperation>();
        var landingContainer = _blobServiceClient.GetBlobContainerClient(_storageOptions.LandingBlobContainerName);

        var result = new ConcurrentBag<FileMetadata>();

        await Parallel.ForEachAsync(filePaths, async (filePath, cancelationToken) =>
        {
            var fileMetadata = await CopyFromUriAsync(landingContainer, filePath, cancelationToken);
            if (fileMetadata is not null)
            {
                result.Add(fileMetadata);
            }
        });

        return result;
    }

    private async Task<FileMetadata> CopyFromUriAsync(BlobContainerClient landingContainer, string filePath, CancellationToken cancelationToken)
    {
        var fileUri = new Uri(filePath);
        var fileName = Path.GetFileName(fileUri.LocalPath);

        var landingBlob = landingContainer.GetBlobClient(fileName);

        var response = await landingBlob.SyncCopyFromUriAsync(fileUri, null, cancelationToken);

        if (response.Value.CopyStatus == CopyStatus.Success)
        {
            var eTag = response.Value.ETag.ToString();
            _logger.LogInformation("Copy status for {fileName}: {copyStatus} with ETag: {eTag}", fileName, CopyStatus.Success, eTag);

            var fileMetadata = new FileMetadata
            {
                Uri = landingBlob.Uri.ToString(),
                Etag = eTag
            };

            return fileMetadata;
        }

        return null;
    }
}
