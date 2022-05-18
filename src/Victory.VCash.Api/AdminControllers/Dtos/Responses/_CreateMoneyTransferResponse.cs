using System;
using System.Collections.Generic;
using Victory.VCash.Api.Controllers.Dtos;
using Victory.VCash.Api.Controllers.Dtos.Responses;

namespace Victory.VCash.Api.AdminControllers.Dtos.Responses
{
    public class _CreateMoneyTransferResponse : BaseResponse
    {
        public MoneyTransferDto MoneyTransfer { get; set; }
    }
}
