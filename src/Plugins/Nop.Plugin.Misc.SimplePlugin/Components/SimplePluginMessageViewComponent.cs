using Microsoft.AspNetCore.Mvc;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.SimplePlugin.Components;
public class SimplePluginMessageViewComponent : NopViewComponent
{
    private readonly ISettingService _settingService;

    public SimplePluginMessageViewComponent(ISettingService settingService)
    {
        _settingService = settingService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var settings = await _settingService.LoadSettingAsync<SimplePluginSettings>();
        return View("~/Plugins/Misc.SimplePlugin/Views/PublicInfo.cshtml", settings);
    }

}
