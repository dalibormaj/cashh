using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Responses;
using Victory.VCash.Api.Controllers;
using Victory.VCash.Api.Controllers.CashierApp;
using Victory.VCash.Api.Extensions;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Application.Services.CashierService;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Api.Controllers.AgentApp
{
    public class CashierController : BaseAgentAppController
    {
        private readonly IAgentService _agentService;
        private readonly ICashierService _cashierService;
        public CashierController(IAgentService agentService, ICashierService cashierService)
        {
            _agentService = agentService;
            _cashierService = cashierService;
        }

        [HttpGet]
        [Route("")]
        public async Task<_RegisterAgentResponse> GetCashier()
        {
            //var a = _cashierService.CreateAccessToken("7f9763f2-68be-45c2-bd44-b18a9a2c33db", "testcashier", "11111");
            //var s = _cashierService.ValidateAccessToken(a);
            return new _RegisterAgentResponse();
        }

        [HttpPost]
        [Route("register")]
        public async Task<_RegisterCashierResponse> RegisterCashier(_RegisterCashierRequest request)
        {
            var agentId = HttpContext.Current().Device?.AgentId;
            if (agentId == null)
                throw new VCashException(ErrorCode.AGENT_DOES_NOT_EXIST);

            var cashier = _cashierService.Register(agentId, request.VenueId, request.UserName, request.Name, request.LastName);
            return new _RegisterCashierResponse()
            {
                CashierId = cashier.CashierId
            };
        }
    }
}
