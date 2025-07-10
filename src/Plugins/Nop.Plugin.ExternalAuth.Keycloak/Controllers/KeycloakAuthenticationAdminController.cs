using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nop.Core;
using Nop.Plugin.ExternalAuth.Keycloak.Models;
using Nop.Services.Configuration;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.ExternalAuth.Keycloak.Controllers
{
    [AutoValidateAntiforgeryToken]
    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    public class KeycloakAuthenticationAdminController : BasePluginController
    {
        private readonly KeycloakSettings _settings;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly INotificationService _notificationService;

        public KeycloakAuthenticationAdminController(
            IOptions<KeycloakSettings> settings,
            ISettingService settingService,
            INotificationService notificationService,
            IStoreContext storeContext)
        {
            _settings = settings.Value;
            _settingService = settingService;
            _storeContext = storeContext;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Configure(bool showtour = false)
        {          
            var model = new ConfigurationModel(
                _settings.Enabled,
                _settings.Authority,
                _settings.ClientId,
                _settings.ClientSecret
            )
            {
                CallbackPath = _settings.CallbackPath,
                AttributeMappings = _settings.AttributeMappings
            };
            return View("~/Plugins/ExternalAuth.Keycloak/Views/Configure.cshtml", _settings);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(KeycloakSettings model)
        {
            if (!ModelState.IsValid)
                return await Configure();

            _settings.Enabled = model.Enabled;
            _settings.Authority = model.Authority;
            _settings.ClientId = model.ClientId;

            // Only update secret if provided
            if (!string.IsNullOrEmpty(model.ClientSecret))
                _settings.ClientSecret = model.ClientSecret;

            _settings.CallbackPath = model.CallbackPath;
            _settings.AttributeMappings = model.AttributeMappings;

            await _settingService.SaveSettingAsync(_settings, _storeContext.GetCurrentStore().Id);

            _notificationService.SuccessNotification("Settings saved successfully");
            return await Configure();
        }
    }
}
