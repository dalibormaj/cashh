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
using System.Collections.Generic;
using System.Linq;

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

        [HttpPost]
        [Route("register")]
        public _RegisterCashierResponse RegisterCashier(_RegisterCashierRequest request)
        {
            var agentUserId = HttpContext.Current().Guardian.UserId;
            var agent = _agentService.GetAgent(agentUserId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            var cashier = _cashierService.Register(agent.AgentId.Value, request.VenueId, request.UserName, request.FirstName, request.LastName, request.Pin);
            return new _RegisterCashierResponse()
            {
                CashierId = cashier.CashierId
            };
        }


        [HttpPost]
        [Route("register-default")]
        public _RegisterCashierResponse RegisterDefaultCashier(_RegisterDefaultCashierRequest request)
        {
            var agentUserId = HttpContext.Current().Guardian.UserId;
            var agent = _agentService.GetAgent(agentUserId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            var venues = _agentService.GetVenues(agent.CompanyId.Value);

            if (!(venues?.Any() ?? false))
                throw new VCashException(ErrorCode.VENUE_CANNOT_BE_FOUND);

            var defaultVenueId = venues[0].VenueId;
            var defaultCashier = _cashierService.Register(agent.AgentId.Value,
                                                          defaultVenueId.Value,
                                                          agent.UserName,
                                                          agent.FirstName,
                                                          agent.LastName,
                                                          request.Pin);

            return new _RegisterCashierResponse()
            {
                CashierId = defaultCashier.CashierId
            };
        }

        [HttpGet("all")]
        public _GetCashiersResponse GetCashiers()
        {
            var agentUserId = HttpContext.Current().Guardian.UserId;
            var agent = _agentService.GetAgent(agentUserId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            var cashiers = _cashierService.GetCashiers(agentId: agent.AgentId.Value);
            var result = new _GetCashiersResponse()
            {
                Cashiers = Mapper.Map<List<_CashierDto>>(cashiers)
            };
            return result;
        }
    }
}
