using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi
{
    public static class PlatormWebSiteApiExtensions
    {
        public static PlatormWebSiteApiSettings GetPlatormWebSiteApiSettings(this IConfiguration configuration)
        {
            PlatormWebSiteApiSettings settings = new PlatormWebSiteApiSettings();
            configuration.GetSection(typeof(PlatormWebSiteApiSettings).Name).Bind(settings);

            return settings;
        }

        public static void AddPlatormWebSiteApi(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetPlatormWebSiteApiSettings();
            services.AddHttpClient<IPlatormWebSiteApiClient, PlatormWebSiteApiClient>(config =>
            {
                config.BaseAddress = new Uri(settings.Url);
            });
        }
    }
}
