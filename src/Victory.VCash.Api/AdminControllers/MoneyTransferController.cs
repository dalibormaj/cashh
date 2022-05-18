using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.AdminControllers.Dtos.Requests;
using Victory.VCash.Api.AdminControllers.Dtos.Responses;
using Victory.VCash.Api.Controllers;
using Victory.VCash.Api.Controllers.Dtos;
using Victory.VCash.Application.Services.MoneyTransferService;

namespace Victory.VCash.Api.AdminControllers
{
    [ApiExplorerSettings(GroupName = ControllerGroupName.ADMIN)]
    [Route("admin/money-transfer")]
    public class MoneyTransferController : BaseController
    {
        private readonly IMoneyTransferService _moneyTransferService;
        public MoneyTransferController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
        }

        [HttpPost]
        [Route("verify")]
        [Authorize(AuthSchema.AZURE_AD)]
        public async Task<_CreateMoneyTransferResponse> VerifyMoneyTransfer(_VerifyMoneyTransferRequest request)
        {
            return new _CreateMoneyTransferResponse();
        }

        [HttpPost]
        [Route("reject")]
        [Authorize(AuthSchema.AZURE_AD)]
        public async Task<_CreateMoneyTransferResponse> RejectMoneyTransfer(_RejectMoneyTransferRequest request)
        {
            return new _CreateMoneyTransferResponse();
        }

        [HttpGet]
        [Route("")]
        [Authorize(AuthSchema.AZURE_AD)]
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
