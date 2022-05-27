using Microsoft.Extensions.DependencyInjection;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Application.Services.CashierService;
using Victory.VCash.Application.Services.MoneyTransferService;
using Victory.VCash.Application.Services.UserService;

namespace Victory.VCash.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddVictoryApplication(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMoneyTransferService, MoneyTransferService>();
            services.AddTransient<IMoneyTransferProvider, MoneyTransferProvider>();
            services.AddTransient<ICashierService, CashierService>();
            services.AddTransient<IAgentService, AgentService>();
        }
    }
}
