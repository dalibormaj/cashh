using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Api.Controllers.Auth.Dtos.Requests;
using Victory.VCash.Api.Controllers.Auth.Dtos.Responses;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Requests;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Application.Services.AuthService;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.Resources;

namespace Victory.VCash.Api.Controllers.Auth
{
    public class DeviceController : BaseAuthController
    {
        private readonly IAuthService _authService;
        public DeviceController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost]
        [Route("code")]
        public CreateDeviceCodeResponse CreateDeviceCode([FromBody] CreateDeviceCodeRequest request)
        {
            var device = _authService.RegisterDevice(new Guid(request.AgentId), request.DeviceName);
            return new CreateDeviceCodeResponse()
            {
                DeviceCode = device.DeviceCode,
                ExpiresInMin = device.DeviceCodeExpiresInMin.Value
            };
        }

        [HttpPost]
        [Route("token")]
        public CreateDeviceTokenResponse CreateDeviceToken([FromBody] CreateDeviceTokenRequest request)
        {
            var result = _authService.CreateDeviceToken(new Guid(request.AgentId), request.DeviceCode);
            if (string.IsNullOrEmpty(result.DeviceToken))
                throw new VCashException(ErrorCode.BAD_REQUEST);

            return new CreateDeviceTokenResponse()
            {
                Token = result.DeviceToken,
                ExpiresAt = result.ExpiresAt
            };
        }

        [HttpPost]
        [Route("validate")]
        public ValidateDeviceTokenResponse ValidateDeviceToken([FromBody] ValidateDeviceTokenRequest request)
        {
            var validationResult = _authService.ValidateDeviceToken(request.Token);
            if (!validationResult.IsValid) 
            {
                if (validationResult.Errors?.Any() ?? false)
                {
                    throw new VCashException(validationResult.Errors);
                }

                throw new VCashException(ErrorCode.INVALID_DEVICE_TOKEN);
            }

            return Mapper.Map<ValidateDeviceTokenResponse>(validationResult);
        }
    }
}
