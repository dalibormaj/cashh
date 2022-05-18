namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses
{
    public class RegisterUserResponse : BaseResponse<RegisterUserResult>
    {
    }

    public class RegisterUserResult
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
    }
}



