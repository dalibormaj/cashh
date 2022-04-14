using System.Threading.Tasks;
using Victory.Network.Application.Services.UserService.Outputs;

namespace Victory.Network.Application.Services.UserService
{
    public interface IUserService
    {
        Task<RegisterUserOutput> RegisterUserAsync(int agentId, string citizenId, string emailVerificationUrl, string email, string mobilePhoneNumber, bool canReceiveMarketingMessages, bool IsPoliticallyExposed);
    }
}
