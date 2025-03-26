using Altera.Ingestion.Core.Exceptions;
using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Models.FhirService;
using Altera.Ingestion.Integration.Messages;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Altera.Ingestion.Core.Services;

public class FhirApiService : IFhirApiService
{
    private readonly HttpClient _httpClient;
    private readonly FhirJsonSerializer _fhirJsonSerializer;
    private readonly ILogger<FhirApiService> _logger;

    public FhirApiService(
        HttpClient httpClient,
        FhirJsonSerializer fhirJsonSerializer,
        ILogger<FhirApiService> logger)
    {
        _httpClient = httpClient;
        _fhirJsonSerializer = fhirJsonSerializer;
        _logger = logger;
    }

    public async Task<Uri> InitiateImportOperationAsync(
        InitiateImportMessage initiateLoadMessage,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(initiateLoadMessage);

        var inputFiles = new List<Parameters.ParameterComponent>();

        foreach (var file in initiateLoadMessage.Files)
        {
            _logger.LogInformation("Adding file with Url:{url} Etag:{eTag}", file.Uri.ToString(), file.Etag);
            inputFiles.Add(new Parameters.ParameterComponent
            {
                Name = "url",
                Value = new FhirUri(file.Uri)
            });
            inputFiles.Add(new Parameters.ParameterComponent
            {
                Name = "etag",
                Value = new FhirUri(file.Etag)
            });
        }

        var importRequest = new Parameters
        {
            Parameter =
            [
                new Parameters.ParameterComponent
                {
                    Name = "inputFormat",
                    Value = new FhirString("application/fhir+ndjson")
                },
                new Parameters.ParameterComponent
                {
                    Name = "mode",
                    Value = new FhirString("IncrementalLoad")
                },
                new Parameters.ParameterComponent
                {
                    Name = "allowNegativeVersions",
                    Value = new FhirBoolean(true)
                },
                new Parameters.ParameterComponent
                {
                    Name = "input",
                    Part = inputFiles
                }
            ]
        };

        var importRequestSerilized = await _fhirJsonSerializer.SerializeToStringAsync(importRequest);

        using var requestContent = new StringContent(importRequestSerilized, Encoding.UTF8, "application/fhir+json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("$import", UriKind.Relative),
            Content = requestContent,
            Headers =
            {
                {  "Prefer", "respond-async" }
            }
        };

        HttpResponseMessage response = null;

        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception exception)
        {
            var content = await TryGetResponseContentAsync(response);

            throw new FhirApiException($"Failed to initiate import opertion with response '{content}'.", exception);
        }

        var contentLocation = response.Content.Headers.ContentLocation;

        return contentLocation;
    }

    public async Task<ImportStatusResponse> GetImportOperationStatusAsync(int importOperationId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"_operations/import/{importOperationId}", UriKind.Relative)
        };

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonConvert.DeserializeObject<ImportStatusResponse>(content);
        }

        return null;
    }

    private async Task<string> TryGetResponseContentAsync(HttpResponseMessage response)
    {
        string content = null;

        try
        {
            content = await response.Content.ReadAsStringAsync();
        }
        catch
        {
            // ignored
        }

        return content;
    }
}
