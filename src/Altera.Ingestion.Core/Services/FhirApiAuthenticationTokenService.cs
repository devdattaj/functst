using Altera.Ingestion.Core.Interfaces;
using Altera.Ingestion.Core.Options;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Options;

namespace Altera.Ingestion.Core.Services;

public class FhirApiAuthenticationTokenService : IFhirApiAuthenticationTokenService
{
    private readonly FhirServiceOptions _options;

    public FhirApiAuthenticationTokenService(IOptions<FhirServiceOptions> options)
    {
        _options = options.Value;
    }

    public async Task<AccessToken> GetAuthenticationTokenAsync()
    {
        var credential = new DefaultAzureCredential();

        var scopes = new[] { $"{_options.ServiceBaseUri}/.default" };

        var accessToken = await credential.GetTokenAsync(new TokenRequestContext(scopes));

        return accessToken;
    }
}
