namespace Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Responses
{
    public class RegisterUserResponse
    {
        public RegisterUserResult Result { get; set; }
        public string SystemMessage { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class RegisterUserResult
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
    }
}



