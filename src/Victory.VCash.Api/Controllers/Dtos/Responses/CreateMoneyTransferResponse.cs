using System;
using System.Collections.Generic;

namespace Victory.VCash.Api.Controllers.Dtos.Responses
{
    public class CreateMoneyTransferResponse : BaseResponse
    {
        public MoneyTransferDto MoneyTransfer { get; set; }
    }
}
