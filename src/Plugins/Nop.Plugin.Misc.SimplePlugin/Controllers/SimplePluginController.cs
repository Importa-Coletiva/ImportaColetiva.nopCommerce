using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.SimplePlugin.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.SimplePlugin.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
public class SimplePluginController : BasePluginController
{
    private readonly ISettingService _settingService;

    public SimplePluginController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    public async Task<IActionResult> Configure()
    {
        var settings = await _settingService.LoadSettingAsync<SimplePluginSettings>();
        var model = new SimplePluginModel
        {
            EnableFeature = settings.EnableFeature,
            WelcomeMessage = settings.WelcomeMessage
        };
        return View("~/Plugins/Misc.SimplePlugin/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Configure(SimplePluginModel model)
    {
        var settings = new SimplePluginSettings
        {
            EnableFeature = model.EnableFeature,
            WelcomeMessage = model.WelcomeMessage
        };
        await _settingService.SaveSettingAsync(settings);
        return RedirectToAction("Configure");
    }
}

