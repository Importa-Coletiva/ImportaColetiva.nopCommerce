using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Shipping.ICSP.Models;
using Nop.Services.Configuration;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Shipping.ICSP.Controllers;
[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class ICSPShippingController : BasePluginController
{
    private readonly ISettingService _settingService;
    private readonly ShippingICSPSettings _settings;
    private readonly IPermissionService _permissionService;

    public ICSPShippingController(
        ISettingService settingService,
        ShippingICSPSettings settings,
        IPermissionService permissionService)
    {
        _settingService = settingService;
        _settings = settings;
        _permissionService = permissionService;
    }

   

    [HttpGet]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SHIPPING_SETTINGS)]
    public IActionResult Configure()
    {
        var settings = _settingService.LoadSetting<ShippingICSPSettings>();
        var model = new ICSPShippingModel
        {
            ApiEndpoint = settings.ApiEndpoint,
            ApiKey = settings.ApiKey,
            ApiTimeout = settings.ApiTimeout,
            UseSandbox = settings.UseSandbox,
            EnableEmptyShippingOption = settings.EnableEmptyShippingOption,
            AdditionalHandlingCharge=settings.AdditionalHandlingCharge,
        };
        return View("~/Plugins/Shipping.ICSP/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SHIPPING_SETTINGS)]
    public IActionResult Configure(ICSPShippingModel model)
    {
        if (!ModelState.IsValid)
            return Configure();

        var settings = new ShippingICSPSettings
        {
            ApiEndpoint = model.ApiEndpoint,
            ApiKey = model.ApiKey,
            ApiTimeout = model.ApiTimeout,
            UseSandbox = model.UseSandbox,
            EnableEmptyShippingOption = model.EnableEmptyShippingOption,
            AdditionalHandlingCharge = model.AdditionalHandlingCharge,
        };
        _settingService.SaveSetting(settings);
        ViewBag.Message = "Settings saved successfully.";
        return Configure();
    }
}
