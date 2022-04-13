using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Victory.Network.Api.Dtos.Requests;
using Victory.Network.Api.Dtos.Responses;
using Victory.Network.Application.Services.UserService;
using Victory.Network.Infrastructure.Common;

namespace Victory.Network.Api.Controllers
{
    [Route("v{version:apiVersion}/transaction")]
    public class TransactionController : BaseController
    {
        private IUserService _userService;
        private IGlobalValidator _validator;
        public TransactionController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("deposit")]
        [Authorize]
        public async Task<DepositResponse> Deposit(DepositRequest request)
        {
            return new DepositResponse();
        }

        [HttpPost]
        [Route("payout")]
        [Authorize]
        public async Task<PayoutResponse> Payout(PayoutRequest request)
        {
            return new PayoutResponse();
        }

        [HttpPost]
        [Route("verify")]
        [Authorize]
        public async Task<VerifyTransactionResponse> VerifyTransaction(VerifyTransactionRequest request)
        {
            return new VerifyTransactionResponse();
        }

        [HttpPost]
        [Route("reject")]
        [Authorize]
        public async Task<RejectTransactionResponse> RejectTransaction(RejectTransactionRequest request)
        {
            return new RejectTransactionResponse();
        }

        [HttpPost]
        [Route("refund")]
        [Authorize]
        public async Task<RefundTransactionResponse> RefundTransaction(RefundTransactionRequest request)
        {
            return new RefundTransactionResponse();
        }
    }
}
