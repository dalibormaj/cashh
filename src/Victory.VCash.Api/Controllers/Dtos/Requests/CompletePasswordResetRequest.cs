namespace Victory.VCash.Api.Controllers.Dtos.Requests
{
    public class CompletePasswordResetRequest
    {
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
