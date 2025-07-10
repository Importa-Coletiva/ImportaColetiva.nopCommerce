using Microsoft.Extensions.Logging;
using Nop.Plugin.ExternalAuth.Keycloak.Models;

namespace Nop.Plugin.ExternalAuth.Keycloak.Services
{
    public class FallbackKeycloakService : IKeycloakService
    {
        private readonly ILogger<FallbackKeycloakService> _logger;

        public FallbackKeycloakService(ILogger<FallbackKeycloakService> logger)
        {
            _logger = logger;
        }

        public Task<KeycloakUserModel> GetUserInfoAsync(string accessToken)
        {
            _logger.LogWarning("Keycloak service unavailable - using fallback");
            return Task.FromResult(new KeycloakUserModel());
        }

        public bool IsAvailable() => false;
    }
}
