using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Victory.Network.Api.Dtos.Requests;
using Victory.Network.Api.Dtos.Responses;

namespace Victory.Network.Api.Controllers.Backoffice
{
    [Route("v{version:apiVersion}/bo/transaction")]
    public class TransactionController : BaseController
    {
        public TransactionController()
        {

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
    }
}
