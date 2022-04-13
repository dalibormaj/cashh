using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddHttpClient<IGuardianClient, GuardianClient>(config =>
            {
                config.BaseAddress = new Uri(settings.Guardian.Url);
            });

            services.AddAuthentication(defaultSchema)
                    .AddScheme<AuthenticationSchemeOptions, GuardianAuthenticationHandler>(AuthSchema.BEARER, o => { })
                    .AddScheme<AuthenticationSchemeOptions, AzureAdAuthenticationHandler>(AuthSchema.AZURE_AD, o => { });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                  .AddAuthenticationSchemes(defaultSchema)
                  .RequireAuthenticatedUser()
                  .Build();
            });
        }
    }
}
