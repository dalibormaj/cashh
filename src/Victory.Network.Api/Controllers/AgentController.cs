using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Network.Api.Dtos.Requests;

namespace Victory.Network.Api.Controllers
{
    [Route("v{version:apiVersion}/agent")]
    public class AgentController : BaseController
    {
        public AgentController()
        {

        }
        
        [HttpGet]
        [Authorize]
        public async Task GetAgent()
        {

        }

        [HttpGet("transactions")]
        [Authorize]
        public async Task GetAgentTransactions([FromBody] GetAgentTransactionsRequest request)
        {
        }

        [HttpGet("commissions")]
        [Authorize]
        public async Task GetCommissions([FromBody] GetCommisionsRequest request)
        {

        }
        
    }
}
