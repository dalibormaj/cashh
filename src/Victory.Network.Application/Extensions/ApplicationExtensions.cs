using Microsoft.Extensions.DependencyInjection;
using Victory.Network.Application.Services.UserService;

namespace Victory.Network.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
        }
    }
}
