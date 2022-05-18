using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.Dtos;
using Victory.VCash.Api.Controllers.Dtos.Requests;
using Victory.VCash.Api.Controllers.Dtos.Responses;
using Victory.VCash.Application.Services.MoneyTransferService;

namespace Victory.VCash.Api.Controllers
{
    [Route("money-transfer")]
    public class MoneyTransferController : BaseController
    {
        private readonly IMoneyTransferService _moneyTransferService;   
        public MoneyTransferController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
        }

        [HttpPost]
        [Route("deposit")]
        [Authorize]
        public async Task<CreateMoneyTransferResponse> DepositAsync(DepositRequest request)
        {
            GlobalValidator.Validate(request);
            var agentId = HttpContext.User.GetId() ?? throw new ArgumentException("AgentId not found");
            var moneyTransfer = await _moneyTransferService.CreateAsync(agentId, request.ToUserId, request.Amount);

            return new CreateMoneyTransferResponse()
            {
                MoneyTransfer = Mapper.Map<MoneyTransferDto>(moneyTransfer)
            };
        }

        [HttpPost]
        [Route("payout")]
        [Authorize]
        public async Task<CreateMoneyTransferResponse> PayoutAsync(PayoutRequest request)
        {
            GlobalValidator.Validate(request);
            var agentId = HttpContext.User.GetId() ?? throw new ArgumentException("AgentId not found");

            var moneyTransfer = await _moneyTransferService.CreateAsync(request.FromUserId, agentId, request.Amount);

            return new CreateMoneyTransferResponse()
            {
                MoneyTransfer = Mapper.Map<MoneyTransferDto>(moneyTransfer)
            };
        }

        [HttpPost]
        [Route("refund/{moneyTransferId}")]
        [Authorize]
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
        [Authorize]
        public GetMoneyTransfersResponse GetMoneyTransfers([FromQuery] GetMoneyTransferFilterRequest filter)
        {
            GlobalValidator.Validate(filter);
            var agentId = HttpContext.User.GetId() ?? throw new ArgumentException("AgentId not found");

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
