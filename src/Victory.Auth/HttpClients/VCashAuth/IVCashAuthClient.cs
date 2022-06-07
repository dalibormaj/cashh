using System.Threading.Tasks;
using Victory.Auth.HttpClients.Guardian.Dtos;
using Victory.Auth.HttpClients.VCashAuth.Dtos;

namespace Victory.Auth.HttpClients.Guardian
{
    public interface IVCashAuthClient
    {
        Task<ValidateDeviceTokenResponse> ValidateDeviceTokenAsync(string token);
    }
}
