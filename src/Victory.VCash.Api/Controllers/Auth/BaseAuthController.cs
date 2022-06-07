using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Victory.Auth;

namespace Victory.VCash.Api.Controllers.Auth
{
    [ApiExplorerSettings(GroupName = ControllerGroupName.AUTH)]
    [Route($"{ControllerGroupName.AUTH}/[controller]")]
    public class BaseAuthController : BaseController
    {
    }
}
