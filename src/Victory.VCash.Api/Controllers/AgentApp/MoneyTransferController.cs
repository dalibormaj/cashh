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

namespace Victory.VCash.Api.Controllers.AgentApp
{
    public class MoneyTransferController : BaseAgentAppController
    {
        private readonly IMoneyTransferService _moneyTransferService;
        public MoneyTransferController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
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
