using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Shipping.ICSP.Component;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Shipping.ICSP;

public class ShippingICSPPlugin : BasePlugin, IWidgetPlugin, IShippingRateComputationMethod
{
    private readonly ISettingService _settingService;
    private readonly IOrderService _orderService;
    private readonly IWorkflowMessageService _workflowMessageService;
    private readonly IWebHelper _webHelper;
    private readonly ShippingICSPSettings _icspSettings;
    private readonly ILocalizationService _localizationService;
    private readonly IGenericAttributeService _genericAttributeService;
    private readonly ILogger _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public bool HideInWidgetList => false;

    public ShippingICSPPlugin(
                ISettingService settingService,
            IOrderService orderService,
            IWorkflowMessageService workflowMessageService,
            ShippingICSPSettings icspSettings,
            ILocalizationService localizationService,
            IGenericAttributeService genericAttributeService,
            ILogger logger,
            IWebHelper webHelper,
            IHttpContextAccessor httpContextAccessor)
    {
        _settingService = settingService;
        _orderService = orderService;
        _workflowMessageService = workflowMessageService;
        _icspSettings = icspSettings;
        _localizationService = localizationService;
        _genericAttributeService = genericAttributeService;
        _logger = logger;
        _webHelper = webHelper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetShippingOptionResponse> GetShippingOptionsAsync(GetShippingOptionRequest getShippingOptionRequest)
    {
        var response = new GetShippingOptionResponse();

        try
        {
            // Simulate API call delay
            await Task.Delay(1500);
            // TODO: Replace with real API call to ICSP Freight Forwarder service
            if (!_icspSettings.EnableEmptyShippingOption)
            {
                response.ShippingOptions.Add(new ShippingOption
                {
                    Name = "ICSP Standard Shipping",
                    Rate = 10.00m,
                    Description = "Delivery in 5-7 business days"
                });

                response.ShippingOptions.Add(new ShippingOption
                {
                    Name = "ICSP Express Shipping",
                    Rate = 25.00m,
                    Description = "Delivery in 1-2 business days"
                });
            }
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync("ICSP API Error", ex);
            response.Errors.Add("ICSP could not retrieve shipping options at this time. Please try again later.");
        }

        return response;
    }


    public override async Task InstallAsync()
    {
        await _settingService.SaveSettingAsync(new ShippingICSPSettings
        {
            ApiEndpoint = "",
            ApiKey = "",
            ApiTimeout = 30,
            UseSandbox = true,
            EnableEmptyShippingOption = false,
            AdditionalHandlingCharge = 0
        });

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Shipping.ICSP.Configuration"] = "ICSP Shipping Configuration",
            ["Plugins.Shipping.ICSP.Fields.ApiEndpoint"] = "API Endpoint",
            ["Plugins.Shipping.ICSP.Fields.ApiKey"] = "API Key",
            ["Plugins.Shipping.ICSP.Fields.ApiTimeout"] = "Timeout (sec)",
            ["Plugins.Shipping.ICSP.Fields.UseSandbox"] = "Enable Sand box mode",
            ["Plugins.Shipping.ICSP.Fields.EnableEmptyShippingOption"] = "Test empty Shipping scenario",
            ["Plugins.Shipping.ICSP.Fields.AdditionalHandlingCharge"] = "Additional Handling Charge(s)",
            ["Plugins.Shipping.ICSP.Save"] = "Save settings",
            ["Plugins.Shipping.ICSP.PleaseWait"] = "Our Freight forwarders are checking the order details to provide the best rates. Please wait."
        });

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await _settingService.DeleteSettingAsync<ShippingICSPSettings>();
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Shipping.ICSP");
        await base.UninstallAsync();
    }

    public Task<decimal?> GetFixedRateAsync(GetShippingOptionRequest getShippingOptionRequest)
    {
        return Task.FromResult<decimal?>(null);
    }

    public Task<IShipmentTracker> GetShipmentTrackerAsync()
    {
        return Task.FromResult<IShipmentTracker>(null);
    }

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/ICSPShipping/Configure";
    }


    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(ICSPShippingWidgetViewComponent);
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.CheckoutShippingMethodTop });
    }
}
