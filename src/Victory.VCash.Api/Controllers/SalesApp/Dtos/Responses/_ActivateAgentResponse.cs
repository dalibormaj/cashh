namespace Victory.VCash.Api.Controllers.SalesApp.Dtos.Responses
{
    public class _ActivateAgentResponse : BaseResponse
    {
        public _DefaultCashier DefaultCashier { get; set; }
    }

    public class _DefaultCashier
    {
        public string UserName { get; set; }
        public string Pin { get; set; }
    }
}
