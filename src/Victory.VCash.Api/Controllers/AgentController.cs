using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.Dtos.Requests;
using Victory.VCash.Api.Controllers.Dtos.Responses;
using Victory.VCash.Application.Services.UserService;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Api.Controllers
{
    [Route("agent")]
    public class AgentController : BaseController
    {
        private readonly IUserService _userService;
        public AgentController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("")]
        [Authorize]
        public async Task<User> GetAgent()
        {
            var agentId = HttpContext.User.GetId() ?? throw new ArgumentException("AgentId not found");
            var agent = await _userService.GetAgentAsync(agentId.ToString());
            return agent;
        }

        //[HttpGet("commissions")]
        //[Authorize]
        //public async Task GetAgentCommissions(GetCommisionsRequest request)
        //{
        //    var agentId = HttpContext.User.GetId() ?? throw new ArgumentException("AgentId not found");
        //}

        //[HttpGet("network")]
        //[Authorize]
        //public async Task GetAgentNetwork()
        //{
        //    var agentId = HttpContext.User.GetId() ?? throw new ArgumentException("AgentId not found");
        //}

        [HttpPost("request-password-reset")]
        public async Task<BaseResponse> RequestPasswordReset(RequestPasswordResetRequest request)
        {
            GlobalValidator.Validate(request);
            await _userService.RequestPasswordResetAsync(request.UserIdentifier, request.PasswordResetUrl);
            return new BaseResponse();
        }

        [HttpPost("complete-password-reset")]
        public async Task<BaseResponse> CompletePasswordReset(CompletePasswordResetRequest request)
        {
            GlobalValidator.Validate(request);
            await _userService.CompletePasswordResetAsync(request.NewPassword, request.Token);
            return new BaseResponse();
        }
    }
}
