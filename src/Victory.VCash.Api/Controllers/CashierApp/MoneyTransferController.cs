using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.CashierApp.Dtos;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Requests;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;
using Victory.VCash.Api.Extensions;
using Victory.VCash.Application.Services.MoneyTransferService;

namespace Victory.VCash.Api.Controllers.CashierApp
{
    public class MoneyTransferController : BaseCashierAppController
    {
        private readonly IMoneyTransferService _moneyTransferService;   
        public MoneyTransferController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
        }

        [HttpPost]
        [Route("deposit")]
        public async Task<CreateMoneyTransferResponse> DepositAsync([FromBody] DepositRequest request)
        {
            GlobalValidator.Validate(request);

            var fromUserId = HttpContext.Current().Device.AgentUserId;
            var cashierId = HttpContext.Current().Cashier?.CashierId;

            var moneyTransfer = await _moneyTransferService.CreateAsync(fromUserId, request.ToUserId, request.Amount, cashierId);

            return new CreateMoneyTransferResponse()
            {
                MoneyTransfer = Mapper.Map<MoneyTransferDto>(moneyTransfer)
            };
        }

        [HttpPost]
        [Route("payout")]
        public async Task<CreateMoneyTransferResponse> PayoutAsync([FromBody] PayoutRequest request)
        {
            GlobalValidator.Validate(request);

            var toUserId = HttpContext.Current().Device.AgentUserId;
            var cashierId = HttpContext.Current().Cashier?.CashierId;

            var moneyTransfer = await _moneyTransferService.CreateAsync(request.FromUserId, toUserId, request.Amount, cashierId);

            return new CreateMoneyTransferResponse()
            {
                MoneyTransfer = Mapper.Map<MoneyTransferDto>(moneyTransfer)
            };
        }

        [HttpPost]
        [Route("refund/{moneyTransferId}")]
        public async Task<CreateMoneyTransferResponse> RefundAsync(int moneyTransferId)
        {
            var moneyTransfer = await _moneyTransferService.RefundAsync(moneyTransferId);
            return new CreateMoneyTransferResponse()
            {
                MoneyTransfer = Mapper.Map<MoneyTransferDto>(moneyTransfer)  
            };
        }

        [HttpGet]
        [Route("")]
        public GetMoneyTransfersResponse GetMoneyTransfers([FromQuery] GetMoneyTransferFilterRequest filter)
        {
            GlobalValidator.Validate(filter);
            var agentId = HttpContext.User.GetUserId() ?? throw new ArgumentException("AgentId not found");

            var moneyTransfers = _moneyTransferService.GetMoneyTransfers(filter.MoneyTransferId,
                                                                         agentId,
                                                                         filter.ToUserId,
                                                                         filter.AmountFrom,
                                                                         filter.AmountTo,
                                                                         filter.DateFrom,
                                                                         filter.DateTo,
                                                                         filter.Status);
            return new GetMoneyTransfersResponse() { MoneyTransfers = Mapper.Map<List<MoneyTransferDto>>(moneyTransfers) };
        }
    }
}
