using System.Threading.Tasks;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi
{
    public interface IPlatformWebSiteApiClient
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);
        Task<CanLoginWithResponse> CanLoginWithAsync(string userName);
        Task<RequestPasswordResetResponse> RequestPasswordResetAsync(RequestPasswordResetRequest request);
        Task<DefaultResponse> CompletePasswordResetAsync(CompletePasswordResetRequest request);
    }
}
