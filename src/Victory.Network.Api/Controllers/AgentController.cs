using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpGet("agentTransactions")]
        [Authorize]
        public async Task GetAgentTransactions()
        {

        }

        [HttpGet("commissions")]
        [Authorize]
        public async Task GetCommissions()
        {

        }
        
    }
}
