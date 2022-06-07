using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Victory.Auth;

namespace Victory.VCash.Api.Controllers.AgentApp
{
    [ApiExplorerSettings(GroupName = ControllerGroupName.AGENT_APP)]
    [Route($"{ControllerGroupName.AGENT_APP}/[controller]")]
    [Authorize(AuthenticationSchemes = AuthScheme.GUARDIAN)]
    public class BaseAgentAppController : BaseController
    {
    }
}
