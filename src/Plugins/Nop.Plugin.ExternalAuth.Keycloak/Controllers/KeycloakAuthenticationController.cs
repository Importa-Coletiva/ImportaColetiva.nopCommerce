using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.ExternalAuth.Keycloak.Models;
using Nop.Plugin.ExternalAuth.Keycloak.Services;
using Nop.Services.Customers;

namespace Nop.Plugin.ExternalAuth.Keycloak.Controllers
{
    public class KeycloakAuthenticationController : Controller
    {
        private readonly IKeycloakService _keycloakService;
        private readonly ICustomerService _customerService;
        protected readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IWorkContext _workContext;
        private readonly ILogger<KeycloakAuthenticationController> _logger;

        public KeycloakAuthenticationController(
            IKeycloakService keycloakService,
            ICustomerService customerService,
            IWorkContext workContext,
            ICustomerRegistrationService customerRegistrationService,
            ILogger<KeycloakAuthenticationController> logger)
        {
            _keycloakService = keycloakService;
            _customerService = customerService;
            _customerRegistrationService = customerRegistrationService;
            _workContext = workContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            try
            {
                if (!_keycloakService.IsAvailable())
                    return RedirectToRoute("Login", new { returnUrl });

                var authenticateResult = await HttpContext.AuthenticateAsync("Keycloak");

                if (!authenticateResult.Succeeded)
                    return Challenge("Keycloak");

                var userInfo = await _keycloakService.GetUserInfoAsync(
                    authenticateResult.Properties.GetTokenValue("access_token"));

                var customer = await SyncCustomerAsync(userInfo);

                await _customerRegistrationService.SignInCustomerAsync(customer, returnUrl, false);

                return RedirectToRoute("Homepage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Keycloak authentication failed");
                return RedirectToRoute("Login", new { returnUrl, error = "keycloak_failed" });
            }
        }

        private async Task<Customer> SyncCustomerAsync(KeycloakUserModel userInfo)
        {
            var customer = await _customerService.GetCustomerByEmailAsync(userInfo.Email) ??
                new Customer { Email = userInfo.Email };

            // Map Keycloak attributes to customer
            customer.Username = userInfo.PreferredUsername;
            customer.FirstName = userInfo.GivenName;
            customer.LastName = userInfo.FamilyName;

            // Custom attribute mapping
            //if (userInfo.AdditionalAttributes.TryGetValue("phone", out var phone))
            //    customer.Phone = phone;

            // Save customer
            if (customer.Id == 0)
                await _customerService.InsertCustomerAsync(customer);
            else
                await _customerService.UpdateCustomerAsync(customer);

            return customer;
        }
    }
}
