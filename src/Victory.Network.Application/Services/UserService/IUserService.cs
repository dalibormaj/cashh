using System.Threading.Tasks;
using Victory.Network.Application.Services.UserService.Outputs;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.Network.Application.Services.UserService
{
    public interface IUserService
    {
        Task<int> RegisterUserAsync(int registedByUserId, string citizenId, string emailVerificationUrl, string email, string mobilePhoneNumber, bool canReceiveMarketingMessages, bool IsPoliticallyExposed);
        Task<GetUserDetailsResponse> GetUser(string identifier);
    }
}
