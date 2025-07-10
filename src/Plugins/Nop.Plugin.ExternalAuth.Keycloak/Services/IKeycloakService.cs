using Nop.Plugin.ExternalAuth.Keycloak.Models;

namespace Nop.Plugin.ExternalAuth.Keycloak.Services
{
    public interface IKeycloakService
    {
        Task<KeycloakUserModel> GetUserInfoAsync(string accessToken);

        bool IsAvailable();
    }
}