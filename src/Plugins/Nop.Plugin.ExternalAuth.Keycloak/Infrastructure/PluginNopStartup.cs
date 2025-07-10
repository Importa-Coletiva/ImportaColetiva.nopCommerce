using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.ExternalAuth.Keycloak.Extensions;
using Nop.Plugin.ExternalAuth.Keycloak.Models;
using Nop.Plugin.ExternalAuth.Keycloak.Services;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;

namespace Nop.Plugin.ExternalAuth.Keycloak.Infrastructure
{
    public class PluginNopStartup : INopStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IExternalAuthenticationMethod, KeycloakExternalAuthMethod>();
            services.AddScoped<KeycloakSettings>();
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            try
            {               
                var keycloakConfig = configuration.GetSection("Keycloak");
                if (keycloakConfig.Exists())
                {
                    services.AddAuthentication()
                        .AddOpenIdConnect("Keycloak", options =>
                        {
                            options.Authority = keycloakConfig["Authority"];
                            options.ClientId = keycloakConfig["ClientId"];
                            options.ClientSecret = keycloakConfig["ClientSecret"];
                            options.ResponseType = "code";
                            options.SaveTokens = true;
                            options.GetClaimsFromUserInfoEndpoint = true;

                            // Error handling
                            options.Events = new OpenIdConnectEvents
                            {
                                OnAuthenticationFailed = context =>
                                {
                                    context.Response.Redirect("/login?error=keycloak_failed");
                                    context.HandleResponse();
                                    return Task.CompletedTask;
                                },
                                OnRemoteFailure = context =>
                                {
                                    context.Response.Redirect("/login?error=keycloak_remote");
                                    context.HandleResponse();
                                    return Task.CompletedTask;
                                }
                            };
                        });
                }
                services.AddScoped<IKeycloakService, KeycloakService>();
            }
            catch (Exception ex)
            {
                // Log error but don't crash application
                services.AddSingleton<IKeycloakService, FallbackKeycloakService>();
            }
            //register services and interfaces
            //services.AddScoped<CustomModelFactory, ICustomerModelFactory>();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            application.UseMiddleware<KeycloakAuthenticationMiddleware>();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 300;
      
    }
}