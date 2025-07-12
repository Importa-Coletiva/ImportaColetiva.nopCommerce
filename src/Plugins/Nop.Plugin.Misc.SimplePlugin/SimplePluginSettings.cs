using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.SimplePlugin;
public class SimplePluginSettings: ISettings
{
    public bool EnableFeature { get; set; }
    public string WelcomeMessage { get; set; }

}
