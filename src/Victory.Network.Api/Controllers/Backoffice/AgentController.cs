using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Victory.Network.Api.Dtos.Requests;
using Victory.Network.Api.Dtos.Responses;

namespace Victory.Network.Api.Controllers.Backoffice
{
    [Route("v{version:apiVersion}/bo/agent")]
    public class AgentController : BaseController
    {
        public AgentController()
        {

        }

        //[HttpPost]
        //[Route("register")]
        //[Authorize]
        //public async Task<DepositResponse> RegisterAgent(DepositRequest request)
        //{
        //    return new DepositResponse();
        //}
    }
}
