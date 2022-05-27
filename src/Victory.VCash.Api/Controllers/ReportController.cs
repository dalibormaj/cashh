using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Victory.Auth;

namespace Victory.VCash.Api.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchema.BEARER)]
    public class ReportController : BaseController
    {
    }
}
