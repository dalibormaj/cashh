using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Responses;
using Victory.VCash.Api.Controllers.CashierApp;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Application.Services.AgentService.Inputs;

namespace Victory.VCash.Api.Controllers.SalesApp
{
    public class AgentController : BaseSalesAppController
    {
        private readonly IAgentService _agentService;
        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<_RegisterAgentResponse> RegisterAgent(_RegisterAgentRequest request)
        {
            var input = Mapper.Map<RegisterAgentInput>(request);
            var result = await _agentService.RegisterAgentAsync(input);

            return new _RegisterAgentResponse()
            {
                AgentId = result.Agent.AgentId?.ToString()
            };
        }
    }
}
