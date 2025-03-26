using Altera.Ingestion.Core.Interfaces;
using Azure.Core;
using System.Net.Http.Headers;

namespace Altera.Ingestion.Core.HttpClientHandlers;

public class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IFhirApiAuthenticationTokenService _authenticationTokenService;

    private AccessToken? _accessToken = null;

    public AuthenticationDelegatingHandler(IFhirApiAuthenticationTokenService authenticationTokenService)
    {
        _authenticationTokenService = authenticationTokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_accessToken == null || DateTimeOffset.UtcNow >= _accessToken.Value.ExpiresOn)
        {
            _accessToken = await _authenticationTokenService.GetAuthenticationTokenAsync();
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Value.Token);

        return await base.SendAsync(request, cancellationToken);
    }
}
