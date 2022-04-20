using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Victory.Network.Infrastructure.HttpClients.InternalApi;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi;
using Victory.Network.Infrastructure.Repositories;

namespace Victory.Network.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static string GetApplicationName(this IConfiguration configuration)
        {
            var appName = configuration.GetSection("ApplicationName")?.Value ?? string.Empty;
            return appName;
        }

        public static void AddVictoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories(configuration);
            services.AddPlatformWebSiteApiClient(configuration);
            services.AddInternalApiClient(configuration);
        }
    }


}
