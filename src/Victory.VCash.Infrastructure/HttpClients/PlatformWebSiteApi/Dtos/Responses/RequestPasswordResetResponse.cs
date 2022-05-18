
namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses
{
    public class RequestPasswordResetResponse : BaseResponse<RequestPasswordResetResult>
    {
    }

    public class RequestPasswordResetResult
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
    }
}
