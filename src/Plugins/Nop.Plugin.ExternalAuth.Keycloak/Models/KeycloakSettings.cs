using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.Keycloak.Models;
public class KeycloakSettings : ISettings
{
    public bool Enabled { get; set; } = true;

    [Required]
    public string Authority { get; set; } = "https://your-keycloak/auth/realms/master";

    [Required]
    public string ClientId { get; set; } = "nopcommerce-client";

    [Required]
    public string ClientSecret { get; set; }

    public string CallbackPath { get; set; } = "/signin-keycloak";

    [DefaultValue(true)]
    public bool ShowLoginButton { get; set; } = true;

    // Attribute mappings
    public List<AttributeMapping> AttributeMappings { get; set; } = new()
    {
        new() { KeycloakClaim = "given_name", NopCustomerAttribute = "FirstName", IsRequired = true },
        new() { KeycloakClaim = "family_name", NopCustomerAttribute = "LastName", IsRequired = true },
        new() { KeycloakClaim = "email", NopCustomerAttribute = "Email", IsRequired = true },
        new() { KeycloakClaim = "phone_number", NopCustomerAttribute = "Phone" }
    };
}
