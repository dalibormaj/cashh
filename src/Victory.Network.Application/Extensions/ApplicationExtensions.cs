using Microsoft.Extensions.DependencyInjection;
using Victory.Network.Application.Services.TransactionService;
using Victory.Network.Application.Services.UserService;

namespace Victory.Network.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddVictoryApplication(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITransactionService, TransactionService>();
        }
    }
}
