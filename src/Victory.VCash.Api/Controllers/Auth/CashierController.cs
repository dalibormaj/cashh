using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Victory.Auth;
using Victory.VCash.Api.Controllers.Auth.Dtos.Requests;
using Victory.VCash.Api.Controllers.Auth.Dtos.Responses;
using Victory.VCash.Api.Extensions;
using Victory.VCash.Application.Services.AuthService;
using Victory.VCash.Application.Services.CashierService;

namespace Victory.VCash.Api.Controllers.Auth
{
    public class CashierController : BaseAuthController
    {
        private readonly IAuthService _authService;
        public CashierController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost]
        [Route("token")]
        [Authorize(AuthenticationSchemes = AuthScheme.VCASH_DEVICE)]
        public CreateCashierTokenResponse CreateCashierToken([FromBody] CreateCashierTokenRequest request)
        {
            var deviceToken = HttpContext.Request.Headers[HeaderNames.Authorization];
            var result = _authService.CreateCashierToken(deviceToken, request.UserName, request.Pin);

            return new CreateCashierTokenResponse()
            {
                AccessToken = result.AccessToken
            };
        }

        [HttpPost]
        [Route("refresh")]
        public RefreshCashierTokenResponse RefreshCashierToken([FromBody] RefreshCashierTokenRequest request)
        {
            return new RefreshCashierTokenResponse();
        }
    }
}
