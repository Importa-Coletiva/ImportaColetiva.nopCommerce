using Newtonsoft.Json;

namespace Nop.Plugin.ExternalAuth.Keycloak.Models
{
    public class KeycloakUserModel
    {
        [JsonProperty("sub")]
        public string SubjectId { get; set; }

        [JsonProperty("preferred_username")]
        public string PreferredUsername { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalAttributes { get; set; }
            = new Dictionary<string, object>();
    }
}
