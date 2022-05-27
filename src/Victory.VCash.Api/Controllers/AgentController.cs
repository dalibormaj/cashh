using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.Dtos.Requests;
using Victory.VCash.Api.Controllers.Dtos.Responses;
using Victory.VCash.Api.Extensions;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Application.Services.UserService;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Resources;

namespace Victory.VCash.Api.Controllers
{
    [Route("agent")]
    [Authorize(AuthenticationSchemes = AuthSchema.BEARER)]
    public class AgentController : BaseController
    {
        private readonly IAgentService _agentService;
        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }


         [HttpGet("")]
        public async Task<GetAgentResponse> GetAgent()
        {
            //var agentId = HttpContext.Current().Agent?. ?? throw new ArgumentException("AgentId not found");
            //var agent = await _agentService.GetAgentAsync("dsadasdas");
            //var response = Mapper.Map<GetAgentResponse>(agent);
            //return response;
            return null;
        }

        [HttpPost("request-password-reset")]
        [AllowAnonymous]
        public async Task<RequestPasswordResetResponse> RequestPasswordReset(RequestPasswordResetRequest request)
        {
            GlobalValidator.Validate(request);
            await _agentService.RequestPasswordResetAsync(request.UserIdentifier, request.PasswordResetUrl);
            return new RequestPasswordResetResponse()
            {
                Message = ResourceManager.GetText("Email successfully sent")
            };
        }

        [HttpPost("complete-password-reset")]
        [AllowAnonymous]
        public async Task<RequestPasswordResetResponse> CompletePasswordReset(CompletePasswordResetRequest request)
        {
            GlobalValidator.Validate(request);
            await _agentService.CompletePasswordResetAsync(request.NewPassword, request.Token);
            return new RequestPasswordResetResponse()
            {
                Message = ResourceManager.GetText("Password successfully changed")
            };
        }
    }
}
