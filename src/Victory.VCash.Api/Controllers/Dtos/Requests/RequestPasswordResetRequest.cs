namespace Victory.VCash.Api.Controllers.Dtos.Requests
{
    public class RequestPasswordResetRequest
    {
        public string UserIdentifier { get; set; }
        public string PasswordResetUrl { get; set; }
    }
}
