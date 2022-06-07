using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Responses;
using Victory.VCash.Api.Controllers;
using Victory.VCash.Api.Controllers.CashierApp;
using Victory.VCash.Api.Controllers.CashierApp.Dtos;
using Victory.VCash.Application.Services.MoneyTransferService;
using Victory.VCash.Api.Controllers.SalesApp;

namespace Victory.VCash.Api.Controlers.SalesApp
{
    public class MoneyTransferController : BaseSalesAppController
    {
        private readonly IMoneyTransferService _moneyTransferService;
        public MoneyTransferController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
        }

        [HttpPost]
        [Route("verify")]
        public async Task<_CreateMoneyTransferResponse> VerifyMoneyTransfer([FromQuery] long moneyTransferId)
        {
            return new _CreateMoneyTransferResponse();
        }

        [HttpPost]
        [Route("reject")]
        public async Task<_CreateMoneyTransferResponse> RejectMoneyTransfer([FromQuery] long moneyTransferId)
        {
            return new _CreateMoneyTransferResponse();
        }

        [HttpGet]
        [Route("")]
        public _GetMoneyTransfersResponse GetMoneyTransfers([FromQuery] _GetMoneyTransferFilterRequest filter)
        {
            GlobalValidator.Validate(filter);
            var moneyTransfers = _moneyTransferService.GetMoneyTransfers(filter.MoneyTransferId,
                                                                         filter.FromUserId,
                                                                         filter.ToUserId,
                                                                         filter.AmountFrom,
                                                                         filter.AmountTo,
                                                                         filter.DateFrom,
                                                                         filter.DateTo,
                                                                         filter.Status);
            return new _GetMoneyTransfersResponse() { MoneyTransfers = Mapper.Map<List<MoneyTransferDto>>(moneyTransfers) };
        }
    }
}
