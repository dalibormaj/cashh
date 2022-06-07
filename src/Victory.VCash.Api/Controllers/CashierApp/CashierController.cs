using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Requests;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;
using Victory.VCash.Api.Extensions;
using Victory.VCash.Application.Services.CashierService;
using Victory.VCash.Application.Services.UserService;

namespace Victory.VCash.Api.Controllers.CashierApp
{
    public class CashierController : BaseCashierAppController
    {
        private readonly ICashierService _cashierService;
        public CashierController(ICashierService cashierService)
        {
            _cashierService = cashierService;
        }
        
        [HttpGet("")]
        public async Task<BaseResponse> GetCashier()
        {
            var cashier = HttpContext.Current().Cashier.UserName;
            return new BaseResponse();
        }
    }
}
