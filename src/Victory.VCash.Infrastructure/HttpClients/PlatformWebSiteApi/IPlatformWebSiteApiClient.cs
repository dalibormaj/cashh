using System.Threading.Tasks;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi
{
    public interface IPlatformWebSiteApiClient
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);
        Task<CanLoginWithResponse> CanLoginWithAsync(string userName);
        Task<OverridePasswordResponse> OverridePasswordAsync(OverridePasswordRequest request, string authToken);
        Task<DefaultResponse> RequestPasswordResetAsync(RequestPasswordResetRequest request);
        Task<DefaultResponse> CompletePasswordResetAsync(CompletePasswordResetRequest request);
        Task<SendEmailVerificationCodeResponse> SendEmailVerificationCodeAsync(SendEmailVerificationCodeRequest request);
        Task<SendSmsVerificationCodeResponse> SendSmsVerificationCodeAsync(SendSmsVerificationCodeRequest request);
        Task<VerifyEmailResponse> VerifyEmailAsync(VerifyEmailRequest request);
        Task<VerifySmsResponse> VerifySmsAsync(VerifySmsRequest request);
    }
}
