using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi;
using Victory.Network.Infrastructure.UnitOfWork;

namespace Victory.Network.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static string GetApplicationName(this IConfiguration configuration)
        {
            var appName = configuration.GetSection("ApplicationName")?.Value ?? string.Empty;
            return appName;
        }

        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccess(configuration);
            services.AddPlatormWebSiteApi(configuration);
        }
    }


}
