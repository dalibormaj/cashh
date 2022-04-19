using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi
{
    internal static class PlatformWebSiteApiExtensions
    {
        public static PlatormWebSiteApiSettings GetPlatformWebSiteApiSettings(this IConfiguration configuration)
        {
            PlatormWebSiteApiSettings settings = new PlatormWebSiteApiSettings();
            configuration.GetSection(typeof(PlatormWebSiteApiSettings).Name).Bind(settings);

            return settings;
        }

        internal static void AddPlatformWebSiteApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetPlatformWebSiteApiSettings();
            services.AddHttpClient<IPlatformWebSiteApiClient, PlatormWebSiteApiClient>(config =>
            {
                config.BaseAddress = new Uri(settings.Url);
            });
        }
    }
}
