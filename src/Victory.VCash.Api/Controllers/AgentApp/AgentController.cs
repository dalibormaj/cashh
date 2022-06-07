using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Responses;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Infrastructure.Resources;

namespace Victory.VCash.Api.Controllers.AgentApp
{
    public class AgentController : BaseAgentAppController
    {

        private readonly IAgentService _agentService;
        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpPost("request-password-reset")]
        [AllowAnonymous]
        public async Task<_RequestPasswordResetResponse> RequestPasswordReset(_RequestPasswordResetRequest request)
        {
            GlobalValidator.Validate(request);
            await _agentService.RequestPasswordResetAsync(request.UserIdentifier, request.PasswordResetUrl);
            return new _RequestPasswordResetResponse()
            {
                Message = ResourceManager.GetText("Email successfully sent")
            };
        }

        [HttpPost("complete-password-reset")]
        [AllowAnonymous]
        public async Task<_RequestPasswordResetResponse> CompletePasswordReset(_CompletePasswordResetRequest request)
        {
            GlobalValidator.Validate(request);
            await _agentService.CompletePasswordResetAsync(request.NewPassword, request.Token);
            return new _RequestPasswordResetResponse()
            {
                Message = ResourceManager.GetText("Password successfully changed")
            };
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<_RegisterAgentResponse> GetAdmin()
        {
            return new _RegisterAgentResponse();
        }
    }
}
