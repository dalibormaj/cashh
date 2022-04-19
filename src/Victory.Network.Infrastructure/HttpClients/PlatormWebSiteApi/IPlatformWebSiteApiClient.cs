using System.Threading.Tasks;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Requests;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Responses;

namespace Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi
{
    public interface IPlatformWebSiteApiClient
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);
        Task<CanLoginWithResponse> CanLoginWithAsync(CanLoginWithRequest request);
    }
}
