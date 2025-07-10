using Nop.Core.Configuration;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.ExternalAuth.Keycloak.Models
{
    public record ConfigurationModel(
        [property: NopResourceDisplayName("Plugins.ExternalAuth.Keycloak.Fields.Enabled")] bool Enabled, 
        [property: NopResourceDisplayName("Plugins.ExternalAuth.Keycloak.Fields.Authority")][property: Required] string Authority, 
        [property: NopResourceDisplayName("Plugins.ExternalAuth.Keycloak.Fields.ClientId")][property: Required] string ClientId, 
        [property: NopResourceDisplayName("Plugins.ExternalAuth.Keycloak.Fields.ClientSecret")][property: DataType(DataType.Password)] string ClientSecret) : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.ExternalAuth.Keycloak.Fields.CallbackPath")]
        public string CallbackPath { get; set; } = "/signin-keycloak";

        // Attribute mapping configuration
        public List<AttributeMapping> AttributeMappings { get; set; } = new();
    }
    
    public class AttributeMapping
    {
        public string KeycloakClaim { get; set; }
        public string NopCustomerAttribute { get; set; }
        public bool IsRequired { get; set; }
    }
}
