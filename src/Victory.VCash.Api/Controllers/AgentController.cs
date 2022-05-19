using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.Dtos.Requests;
using Victory.VCash.Api.Controllers.Dtos.Responses;
using Victory.VCash.Application.Services.UserService;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Resources;

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
           var agentId = HttpContext.User.GetUserId() ?? throw new ArgumentException("AgentId not found");
            var agent = await _userService.GetAgentAsync(agentId.ToString());
            return agent;
        }

        [HttpPost("request-password-reset")]
        public async Task<RequestPasswordResetResponse> RequestPasswordReset(RequestPasswordResetRequest request)
        {
            GlobalValidator.Validate(request);
            await _userService.RequestPasswordResetAsync(request.UserIdentifier, request.PasswordResetUrl);
            return new RequestPasswordResetResponse()
            {
                Message = ResourceManager.GetText("Email successfully sent")
            };
        }

        [HttpPost("complete-password-reset")]
        public async Task<RequestPasswordResetResponse> CompletePasswordReset(CompletePasswordResetRequest request)
        {
            GlobalValidator.Validate(request);
            await _userService.CompletePasswordResetAsync(request.NewPassword, request.Token);
            return new RequestPasswordResetResponse()
            {
                Message = ResourceManager.GetText("Password successfully changed")
            };
        }
    }
}
