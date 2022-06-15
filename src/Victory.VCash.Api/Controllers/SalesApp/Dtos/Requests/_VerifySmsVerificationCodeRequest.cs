namespace Victory.VCash.Api.Controllers.SalesApp.Dtos.Requests
{
    public class _VerifySmsVerificationCodeRequest
    {
        public string AgentId { get; set; }
        public string VerificationCode { get; set; }
    }
}
