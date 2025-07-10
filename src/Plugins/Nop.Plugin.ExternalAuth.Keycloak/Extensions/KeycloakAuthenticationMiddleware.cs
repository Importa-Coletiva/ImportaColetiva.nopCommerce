using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Nop.Plugin.ExternalAuth.Keycloak.Services;

namespace Nop.Plugin.ExternalAuth.Keycloak.Extensions
{
    public class KeycloakAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IKeycloakService _keycloakService;

        public KeycloakAuthenticationMiddleware(
            RequestDelegate next,
            IKeycloakService keycloakService)
        {
            _next = next;
            _keycloakService = keycloakService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (_keycloakService.IsAvailable() &&
                    context.Request.Path.StartsWithSegments("/keycloak-auth"))
                {
                    // Handle Keycloak authentication
                    await HandleKeycloakAuthentication(context);
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/login?error=auth_error");
                return;
            }

            await _next(context);
        }
        private async Task HandleKeycloakAuthentication(HttpContext context)
        {
            if (!_keycloakService.IsAvailable())
            {
                context.Response.Redirect("/login?error=keycloak_unavailable");
                return;
            }

            try
            {
                var result = await context.AuthenticateAsync("Keycloak");

                if (!result.Succeeded)
                {
                    await context.ChallengeAsync("Keycloak");
                    return;
                }

                var userInfo = await _keycloakService.GetUserInfoAsync(
                    result.Properties.GetTokenValue("access_token"));

                // Store user info for controller to process
                context.Items["KeycloakUserInfo"] = userInfo;
                context.Request.Path = "/keycloak-auth/callback";

                await _next(context);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Keycloak authentication failed");
                context.Response.Redirect("/login?error=auth_error");
            }
        }
    }
}
