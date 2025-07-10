using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nop.Plugin.ExternalAuth.Keycloak.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.ExternalAuth.Keycloak.Components
{
    [ViewComponent(Name = "KeycloakAuthentication")]
    public class KeycloakViewComponent : NopViewComponent
    {
        private readonly KeycloakSettings _settings;

        public KeycloakViewComponent(IOptions<KeycloakSettings> settings)
        {
            _settings = settings.Value;
        }
        public IViewComponentResult Invoke()
        {
            if (!_settings.Enabled)
                return Content("");

            return View("~/Plugins/ExternalAuth.Keycloak/Views/PublicInfo.cshtml");
        }

    }
}
