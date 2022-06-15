using System.Collections.Generic;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;

namespace Victory.VCash.Api.Controllers.SalesApp.Dtos.Responses
{
    public class _RegisterAgentResponse : BaseResponse
    {
        public string AgentId { get; set; }
        public _DefaultCashier DefaultCashier { get; set; }
    }
}
