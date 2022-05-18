using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Victory.VCash.Infrastructure.Consumers;
using Victory.VCash.Infrastructure.HttpClients.InternalApi;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi;
using Victory.VCash.Infrastructure.Repositories;

namespace Victory.VCash.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static string GetApplicationName(this IConfiguration configuration)
        {
            var appName = configuration.GetSection("ApplicationName")?.Value ?? string.Empty;
            return appName;
        }

        //Add infrastructure logic (database/http clients/mq consumers/etc.)
        public static void AddVictoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //data access
            services.AddRepositories(configuration);

            //http clients
            services.AddPlatformWebSiteApiClient(configuration);
            services.AddInternalApiClient(configuration);
        }
    }


}
