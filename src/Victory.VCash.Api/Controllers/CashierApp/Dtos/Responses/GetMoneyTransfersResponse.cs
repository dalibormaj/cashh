using System;
using System.Collections.Generic;

namespace Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses
{
    public class GetMoneyTransfersResponse : BaseResponse
    {
        public List<MoneyTransferDto> MoneyTransfers { get; set; }
    }
}
