using System.Diagnostics;
using Nop.Core;
using Nop.Plugin.Misc.SimplePlugin.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.SimplePlugin;

public class SimplePlugin : BasePlugin, IWidgetPlugin
{
    public const string ContactUsBottom = "contactus_bottom";

    protected readonly IWebHelper _webHelper;
    protected readonly ISettingService _settingService;

    public bool HideInWidgetList => false;

    public SimplePlugin(IWebHelper webHelper, ISettingService settingService)
    {
        _webHelper = webHelper;
        _settingService = settingService;
    }

    //public IList<string> GetWidgetZones() => new List<string> { PublicWidgetZones.ContactUsBottom };


    //public string GetWidgetViewComponentName(string widgetZone) => "SimplePluginMessage";

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/SimplePlugin/Configure";
    }

    public override async Task InstallAsync()
    {
        var settings = new SimplePluginSettings
        {
            EnableFeature = true,
            WelcomeMessage = "Welcome to our store!"
        };

        await _settingService.SaveSettingAsync(settings);
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await _settingService.DeleteSettingAsync<SimplePluginSettings>();
        await base.UninstallAsync();
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.ContactUsBottom });
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(SimplePluginMessageViewComponent);
    }
}
