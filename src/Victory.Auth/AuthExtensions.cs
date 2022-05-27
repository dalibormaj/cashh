using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        public static void AddVictoryAuth(this IServiceCollection services, IConfiguration configuration)
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

            var defaultSchema = $"{AuthSchema.BEARER},{AuthSchema.AZURE_AD}";
            services.AddAuthentication(defaultSchema)
                    .AddScheme<AuthenticationSchemeOptions, GuardianAuthenticationHandler>(AuthSchema.BEARER, AuthSchema.BEARER, o => { })
                    .AddScheme<AuthenticationSchemeOptions, AzureAdAuthenticationHandler>(AuthSchema.AZURE_AD, AuthSchema.AZURE_AD, o => { })
                    .AddPolicyScheme(defaultSchema, defaultSchema, options =>
                    {
                        // runs on each request
                        options.ForwardDefaultSelector = context =>
                        {
                            // filter by auth type
                            string authorization = context.Request.Headers[HeaderNames.Authorization];
                            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith(AuthSchema.AZURE_AD, StringComparison.OrdinalIgnoreCase))
                                return AuthSchema.AZURE_AD;

                            // otherwise always check for Bearer (Guardian) auth
                            return AuthSchema.BEARER;
                        };
                    });
        }
    }
}