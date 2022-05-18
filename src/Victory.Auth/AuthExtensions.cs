using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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

        public static void AddVictoryAuth(this IServiceCollection services, IConfiguration configuration, string defaultSchema = AuthSchema.BEARER)
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

            services.AddAuthentication(defaultSchema)
                    .AddScheme<AuthenticationSchemeOptions, GuardianAuthenticationHandler>(AuthSchema.BEARER, o => { })
                    .AddScheme<AuthenticationSchemeOptions, AzureAdAuthenticationHandler>(AuthSchema.AZURE_AD, o => { });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                  .AddAuthenticationSchemes(defaultSchema)
                  .RequireAuthenticatedUser()
                  .Build();

                options.AddPolicy(AuthSchema.BEARER, builder =>
                {
                    builder.AddAuthenticationSchemes(AuthSchema.BEARER);
                    builder.RequireAuthenticatedUser();
                    builder.Build();
                });

                options.AddPolicy(AuthSchema.AZURE_AD, builder =>
                {
                    builder.AddAuthenticationSchemes(AuthSchema.AZURE_AD);
                    builder.RequireAuthenticatedUser();
                    builder.Build();
                });
            });
        }
    }
}