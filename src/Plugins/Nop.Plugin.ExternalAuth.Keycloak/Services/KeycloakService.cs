using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nop.Plugin.ExternalAuth.Keycloak.Extensions;
using Nop.Plugin.ExternalAuth.Keycloak.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Nop.Plugin.ExternalAuth.Keycloak.Services
{
    public class KeycloakService : IKeycloakService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<KeycloakService> _logger;
        private readonly KeycloakSettings _settings;

        public KeycloakService(
            IHttpClientFactory httpClientFactory,
            IOptions<KeycloakSettings> settings,
            ILogger<KeycloakService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<KeycloakUserModel> GetUserInfoAsync(string accessToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("keycloak");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync($"{_settings.Authority}/protocol/openid-connect/userinfo");

                response.EnsureSuccessStatusCode();

                var userInfo = await response.Content.ReadFromJsonAsync<KeycloakUserModel>();
                return userInfo ?? throw new Exception("Null user info received");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Keycloak user info request failed");
                throw new KeycloakServiceException("Failed to get user info", ex);
            }
        }

        // Fallback method
        public bool IsAvailable() => !string.IsNullOrEmpty(_settings?.Authority);
    }
}
