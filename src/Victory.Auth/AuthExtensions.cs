using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using System;
using Victory.Auth.HttpClients.Guardian;

namespace Victory.Auth
{
    public static class AuthExtensions
    {
        public static AuthSettings GetAuthSettings(this IConfiguration configuration)
        {
            AuthSettings settings = new AuthSettings();
            configuration.GetSection(typeof(AuthSettings).Name).Bind(settings);

            return settings;
        }

        public static void AddVictoryAuth(this IServiceCollection services, IConfiguration configuration, string defaultScheme = AuthScheme.GUARDIAN)
        {
            var settings = configuration.GetAuthSettings();

            //guardian
            services.AddHttpClient<IGuardianClient, GuardianClient>(config =>
            {
                config.BaseAddress = new Uri(settings.Guardian.Url);
            });

            //azure AD
            string stsDiscoveryEndpoint = settings.AzureAd.OpenIdConfigUrl;
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());
            OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;
            services.AddSingleton(config);

            //vcash
            services.AddSingleton(settings.VCash);
            services.AddHttpClient<IVCashAuthClient, VCashAuthClient>(config =>
            {
                config.BaseAddress = new Uri(settings.VCash.Url);
            });

            
            //services.AddAuthorization(x =>
            //{
            //    x.DefaultPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(defaultScheme)
            //                                                      .RequireAuthenticatedUser()
            //                                                      .Build();
            //});
            
            services.AddAuthentication()
                    .AddScheme<AuthenticationSchemeOptions, GuardianAuthenticationHandler>(AuthScheme.GUARDIAN, o => { })
                    .AddScheme<AuthenticationSchemeOptions, AzureAdAuthenticationHandler>(AuthScheme.AZURE_AD, o => { })
                    .AddScheme<AuthenticationSchemeOptions, VCashDeviceAuthenticationHandler>(AuthScheme.VCASH_DEVICE, o => { })
                    .AddScheme<AuthenticationSchemeOptions, VCashCashierAuthenticationHandler>(AuthScheme.VCASH_CASHIER, o => { });
        }
    }
}