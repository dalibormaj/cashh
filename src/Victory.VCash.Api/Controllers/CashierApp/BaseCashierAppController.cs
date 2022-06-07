using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Victory.Auth;

namespace Victory.VCash.Api.Controllers.CashierApp
{
    [ApiExplorerSettings(GroupName = ControllerGroupName.CASHIER_APP)]
    [Route($"{ControllerGroupName.CASHIER_APP}/[controller]")]
    [Authorize(AuthenticationSchemes = AuthScheme.VCASH_CASHIER)]
    public class BaseCashierAppController : BaseController
    {
    }
}
