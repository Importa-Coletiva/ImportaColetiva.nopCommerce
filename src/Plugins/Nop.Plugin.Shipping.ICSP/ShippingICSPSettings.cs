using Nop.Core.Configuration;

namespace Nop.Plugin.Shipping.ICSP;
public class ShippingICSPSettings: ISettings
{
    public string ApiEndpoint { get; set; }
    public string ApiKey { get; set; }
    public int ApiTimeout { get; set; }
    public bool UseSandbox { get; set; }
    public bool EnableEmptyShippingOption { get; set; }
    public decimal AdditionalHandlingCharge { get; set; }
}
