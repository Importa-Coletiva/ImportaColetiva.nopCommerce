namespace Nop.Plugin.ExternalAuth.Keycloak.Extensions
{
    public class KeycloakServiceException : Exception
    {
        public KeycloakServiceException() { }
        public KeycloakServiceException(string message) : base(message) { }
        public KeycloakServiceException(string message, Exception inner)
            : base(message, inner) { }
    }
}
