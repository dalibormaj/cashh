namespace Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Responses
{
    public class RegisterUserResponse : DefaultResponse<RegisterUserResult>
    {
    }

    public class RegisterUserResult
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
    }
}



