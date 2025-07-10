using Nop.Plugin.ExternalAuth.Keycloak.Components;
using Nop.Plugin.ExternalAuth.Keycloak.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Plugins;

namespace Nop.Plugin.ExternalAuth.Keycloak.Services;

public class KeycloakExternalAuthMethod : BasePlugin, IExternalAuthenticationMethod
{
    private readonly KeycloakSettings _settings;
    private readonly ISettingService _settingService;

    public KeycloakExternalAuthMethod(KeycloakSettings settings, ISettingService settingService)
    {
        _settings = settings;
        _settingService = settingService;
    }

    public bool IsMethodActive() => _settings.Enabled;

    
    public override async Task InstallAsync()
    {
        // Create default settings
        var settings = new KeycloakSettings
        {
            Enabled = true,
            Authority = "https://your-keycloak/auth/realms/master",
            ClientId = "nopcommerce-client",
            CallbackPath = "/signin-keycloak"
        };

        await _settingService.SaveSettingAsync(settings);

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        // Remove settings
        await _settingService.DeleteSettingAsync<KeycloakSettings>();
        await base.UninstallAsync();
    }

    public Type GetPublicViewComponent()
    {
        return typeof(KeycloakViewComponent);
    }
}
