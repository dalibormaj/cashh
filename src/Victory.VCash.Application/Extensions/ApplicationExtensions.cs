using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Application.Services.AuthService;
using Victory.VCash.Application.Services.CashierService;
using Victory.VCash.Application.Services.MoneyTransferService;
using Victory.VCash.Application.Services.UserService;
using Victory.VCash.Infrastructure.Repositories.Settings;

namespace Victory.VCash.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static AuthServiceSettings GetAuthServiceSettings(this IConfiguration configuration)
        {
            AuthServiceSettings settings = new AuthServiceSettings();
            configuration.GetSection(typeof(AuthServiceSettings).Name).Bind(settings);

            return settings;
        }

        public static void AddVictoryApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var authServiceSettings = configuration.GetAuthServiceSettings();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMoneyTransferService, MoneyTransferService>();
            services.AddTransient<IMoneyTransferProvider, MoneyTransferProvider>();
            services.AddTransient<ICashierService, CashierService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IAuthService>(x => ActivatorUtilities.CreateInstance<AuthService>(x, new object[] { authServiceSettings }));
        }
    }
}
