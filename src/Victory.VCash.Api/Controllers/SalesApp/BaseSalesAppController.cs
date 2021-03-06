using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Victory.Auth;

namespace Victory.VCash.Api.Controllers.SalesApp
{
    [ApiExplorerSettings(GroupName = ControllerGroupName.SALES_APP)]
    [Route($"{ControllerGroupName.SALES_APP}/[controller]")]
    [Authorize(AuthenticationSchemes = AuthScheme.AZURE_AD)]
    public class BaseSalesAppController : BaseController
    {
    }
}
