using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Victory.Network.Infrastructure.HttpClients.InternalApi
{
    internal static class InternalApiExtensions
    {
        public static InternalApiSettings GetInternalApiSettings(this IConfiguration configuration)
        {
            InternalApiSettings settings = new InternalApiSettings();
            configuration.GetSection(typeof(InternalApiSettings).Name).Bind(settings);

            return settings;
        }

        internal static void AddInternalApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetInternalApiSettings();
            services.AddSingleton(x => settings);

            services.AddHttpClient<IInternalApiClient, InternalApiClient>(config =>
            {
                config.BaseAddress = new Uri(settings.Url);
            });
        }
    }
}
