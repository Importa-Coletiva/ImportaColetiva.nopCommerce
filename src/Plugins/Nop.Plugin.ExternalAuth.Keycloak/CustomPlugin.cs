using Nop.Plugin.ExternalAuth.Keycloak.Components;
using Nop.Plugin.ExternalAuth.Keycloak.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Plugins;

namespace Nop.Plugin.ExternalAuth.Keycloak
{
    /// <summary>
    /// Rename this file and change to the correct type
    /// </summary>
    //public class CustomPlugin : BasePlugin, IExternalAuthenticationMethod
    //{
    //    private readonly ISettingService _settingService;

    //    public CustomPlugin(ISettingService settingService)
    //    {
    //        _settingService = settingService;
    //    }

    //    /// <summary>
    //    /// Gets a view component for displaying plugin in public store ("payment info" checkout step)
    //    /// </summary>
    //    public Type GetPublicViewComponent()
    //    {
    //        return typeof(KeycloakViewComponent);
    //    }

    //    public override async Task InstallAsync()
    //    {
    //        // Create default settings
    //        var settings = new KeycloakSettings
    //        {
    //            Enabled = true,
    //            Authority = "https://your-keycloak/auth/realms/master",
    //            ClientId = "nopcommerce-client",
    //            CallbackPath = "/signin-keycloak"
    //        };

    //        await _settingService.SaveSettingAsync(settings);

    //        await base.InstallAsync();
    //    }

    //    public override async Task UninstallAsync()
    //    {
    //        // Remove settings
    //        await _settingService.DeleteSettingAsync<KeycloakSettings>();
    //        await base.UninstallAsync();
    //    }
    //}
}