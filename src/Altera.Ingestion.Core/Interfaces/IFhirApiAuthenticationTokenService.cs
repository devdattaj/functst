using Azure.Core;

namespace Altera.Ingestion.Core.Interfaces;

public interface IFhirApiAuthenticationTokenService
{
    Task<AccessToken> GetAuthenticationTokenAsync();
}
