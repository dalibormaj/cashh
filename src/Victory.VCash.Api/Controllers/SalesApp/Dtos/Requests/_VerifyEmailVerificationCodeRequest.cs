namespace Victory.VCash.Api.Controllers.SalesApp.Dtos.Requests
{
    public class _VerifyEmailVerificationCodeRequest
    {
        public string AgentId { get; set; }
        public string VerificationCode { get; set; }
    }
}
