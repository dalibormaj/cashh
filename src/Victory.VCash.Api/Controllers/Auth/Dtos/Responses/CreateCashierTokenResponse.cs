namespace Victory.VCash.Api.Controllers.Auth.Dtos.Responses
{
    public class CreateCashierTokenResponse : BaseResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
