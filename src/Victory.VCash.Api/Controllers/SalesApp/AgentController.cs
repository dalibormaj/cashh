using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.SalesApp.Dtos.Requests;
using Victory.VCash.Api.Controllers.SalesApp.Dtos.Responses;
using Victory.VCash.Api.Extensions;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Application.Services.AgentService.Inputs;
using Victory.VCash.Application.Services.CashierService;
using Victory.VCash.Infrastructure.Common;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Api.Controllers.SalesApp
{
    public class AgentController : BaseSalesAppController
    {
        private readonly IAgentService _agentService;
        private readonly ICashierService _cashierService;
        public AgentController(IAgentService agentService, ICashierService cashierService)
        {
            _agentService = agentService;
            _cashierService = cashierService;
        }

        [HttpGet]
        [Route("")]
        public async Task<_RegisterAgentResponse> GetAgent(_RegisterAgentRequest request)
        {
            return null;
        }

        [HttpGet]
        [Route("all")]
        public async Task<_RegisterAgentResponse> GetAgents(_RegisterAgentRequest request)
        {
            return null;
        }

        [HttpPost]
        [Route("register")]
        public async Task<_RegisterAgentResponse> RegisterAgentAsync(_RegisterAgentRequest request)
        {
            //format phone number
            if (MobileNumberHelper.TryFormat(request.PhoneNumber, out string phoneNumber))
            {
                request.PhoneNumber = phoneNumber;
            }

            var email = HttpContext.Current().AzureAd.UserName;
            var parentAgent = _agentService.GetMasterAgent(email);

            var input = Mapper.Map<RegisterAgentInput>(request);
            input.Agent.ParentAgentId = parentAgent?.AgentId;
            var result = await _agentService.RegisterAgentAsync(input);

            if (request.ActivateAgent)
            {
                var activateResult = await ActivateAgentAsync(new _ActivateAgentRequest()
                {
                    AgentId = result.Agent.AgentId.ToString(),
                    EmailVerificationUrl = request.EmailVerificationUrl
                });
            }

            return new _RegisterAgentResponse()
            {
                AgentId = result.Agent.AgentId?.ToString(),
            };
        }

        [HttpPost]
        [Route("activate")]
        public async Task<_ActivateAgentResponse> ActivateAgentAsync([FromBody] _ActivateAgentRequest request)
        {
            GlobalValidator.Validate(request);
            var result = await _agentService.ActivateAgentAsync(new Guid(request.AgentId), request.EmailVerificationUrl);

            return new _ActivateAgentResponse()
            {
                
            };
        }

        [HttpPost]
        [Route("register/me-as-master")]
        public async Task<_RegisterMasterAgentResponse> RegisterMasterAgent()
        {
            var email = HttpContext.Current().AzureAd.UserName;
            var fullName = HttpContext.Current().AzureAd.Name.Split(" ");
            var firstName = string.Empty;
            var lastName = string.Empty;
            if (fullName.Length > 0)
                firstName = fullName[0];
            if (fullName.Length > 1)
                lastName = fullName[1];

            var result = await _agentService.RegisterMasterAgentAsync(email, email, firstName, lastName);
            return new _RegisterMasterAgentResponse()
            {
                AgentId = result.Agent.AgentId?.ToString()
            };
        }

        [HttpPost]
        [Route("verification/send-email")]
        public async Task<BaseResponse> SendEmailVerificationCode([FromBody] _SendEmailVerificationCodeRequest request)
        {
            GlobalValidator.Validate(request);
            await _agentService.SendEmailVerificationCode(new Guid(request.AgentId));

            return new BaseResponse();
        }

        [HttpPost]
        [Route("verification/verify-email")]
        [AllowAnonymous]
        public async Task<BaseResponse> VerifyEmailCode([FromBody] _VerifyEmailVerificationCodeRequest request)
        {
            GlobalValidator.Validate(request);
            await _agentService.VerifyEmail(new Guid(request.AgentId), request.VerificationCode);

            return new BaseResponse();
        }

        [HttpPost]
        [Route("verification/send-sms")]
        public async Task<BaseResponse> SendSmsVerificationCode([FromBody] _SendSmsVerificationCodeRequest request)
        {
            GlobalValidator.Validate(request);
            await _agentService.SendSmsVerificationCode(new Guid(request.AgentId));

            return new BaseResponse();
        }

        [HttpPost]
        [Route("verification/verify-sms")]
        [AllowAnonymous]
        public async Task<BaseResponse> VerifySmsCode([FromBody] _VerifySmsVerificationCodeRequest request)
        {
            GlobalValidator.Validate(request);
            await _agentService.VerifySms(new Guid(request.AgentId), request.VerificationCode);

            return new BaseResponse();
        }

    }
}
