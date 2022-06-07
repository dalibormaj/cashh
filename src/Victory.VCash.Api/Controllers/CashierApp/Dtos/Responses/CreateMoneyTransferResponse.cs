using System;
using System.Collections.Generic;

namespace Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses
{
    public class CreateMoneyTransferResponse : BaseResponse
    {
        public MoneyTransferDto MoneyTransfer { get; set; }
    }
}
