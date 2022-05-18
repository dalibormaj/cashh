using System.Threading.Tasks;
using Victory.VCash.Application.Services.UserService.Outputs;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.VCash.Application.Services.UserService
{
    public interface IUserService
    {
        Task<int> RegisterUserAsync(int registedByUserId, string citizenId, string emailVerificationUrl, string email, string mobilePhoneNumber, bool canReceiveMarketingMessages, bool IsPoliticallyExposed);
        Task<User> GetUserAsync(string identifier);
        Task<User> GetAgentAsync(string identifier);
        Task RequestPasswordResetAsync(string userIdentifier, string passwordResetUrl);
        Task CompletePasswordResetAsync(string newPassword, string token);
    }
}
