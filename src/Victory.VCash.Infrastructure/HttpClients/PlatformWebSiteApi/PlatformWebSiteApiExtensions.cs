using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi
{
    internal static class PlatformWebSiteApiExtensions
    {
        public static PlatformWebSiteApiSettings GetPlatformWebSiteApiSettings(this IConfiguration configuration)
        {
            PlatformWebSiteApiSettings settings = new PlatformWebSiteApiSettings();
            configuration.GetSection(typeof(PlatformWebSiteApiSettings).Name).Bind(settings);

            return settings;
        }

        internal static void AddPlatformWebSiteApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetPlatformWebSiteApiSettings();
            services.AddHttpClient<IPlatformWebSiteApiClient, PlatformWebSiteApiClient>(config =>
            {
                config.BaseAddress = new Uri(settings.Url);
            });
        }
    }
}
