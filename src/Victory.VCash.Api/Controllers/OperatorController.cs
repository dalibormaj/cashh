using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.Dtos.Requests;
using Victory.VCash.Api.Controllers.Dtos.Responses;
using Victory.VCash.Api.Extensions;
using Victory.VCash.Application.Services.UserService;

namespace Victory.VCash.Api.Controllers
{
    [Route("operator")]
    public class OperatorController : BaseController
    {
        private readonly IUserService _userService;
        public OperatorController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("")]
        [Authorize]
        public async Task<BaseResponse> GetOperator()
        {
            var user = HttpContext.GetCurrentOperator();
            return new BaseResponse();
        }
    }
}
