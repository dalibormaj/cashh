using System;
using System.Collections.Generic;
using Victory.VCash.Api.Controllers.CashierApp.Dtos;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;

namespace Victory.VCash.Api.Controllers.AgentApp.Dtos.Responses
{
    public class _CreateMoneyTransferResponse : BaseResponse
    {
        public MoneyTransferDto MoneyTransfer { get; set; }
    }
}
