using System.Threading.Tasks;
using Victory.Auth.HttpClients.Guardian.Dtos;

namespace Victory.Auth.HttpClients.Guardian
{
    public interface IGuardianClient
    {
        Task<ValidateTokenResponse> ValidateTokenAsync(string token);
    }
}
