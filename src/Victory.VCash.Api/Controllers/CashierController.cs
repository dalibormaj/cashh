using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.Dtos.Requests;
using Victory.VCash.Api.Controllers.Dtos.Responses;
using Victory.VCash.Api.Extensions;
using Victory.VCash.Application.Services.CashierService;
using Victory.VCash.Application.Services.UserService;

namespace Victory.VCash.Api.Controllers
{
    [Route("cashier")]
    [Authorize(AuthenticationSchemes = AuthSchema.BEARER)]
    public class CashierController : BaseController
    {
        private readonly ICashierService _cashierService;
        public CashierController(ICashierService cashierService)
        {
            _cashierService = cashierService;
        }
        
        [HttpGet("")]
        public async Task<BaseResponse> GetCashier()
        {
            HttpContext.Current().ValidateCashier();
            var cashier = HttpContext.Current().Cashier.UserName;
            return new BaseResponse();
        }

        [HttpPost]
        [Route("token")]
        public string CreateAccessToken(CreateCashierAccessTokenRequest request)
        {
            var agentId = HttpContext.Current().Agent.AgentId;
            return _cashierService.CreateAccessToken(agentId, request.UserName, request.Pin);
        }
    }
}
