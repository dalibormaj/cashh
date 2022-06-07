using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Responses;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses;

namespace Victory.VCash.Application.Services.AgentService
{
    internal static class ErrorMapperExtensions
    {
        public static List<Error> GetErrors(this RegisterUserResponse response)
        {
            if (response.Result != null)
                return null;

            var errors = new List<Error>();
            var errorMessage = string.IsNullOrEmpty(response.ResponseMessage) ? null : $"{response.ResponseMessage}";
            errors.Add(response.ResponseCode switch
            {
                20214 => new Error(ErrorCode.MOBILE_NUMBER_ALREADY_IN_USE),
                20217 => new Error(ErrorCode.INVALID_MOBILE_NUMBER),
                20812 => new Error(ErrorCode.INVALID_EMAIL),
                20100 => new Error(ErrorCode.AGENT_REGISTRATION_FAILED),
                _ => new Error(ErrorCode.SYSTEM_ERROR, errorMessage)
            });
            return errors;
        }

        public static void ThrowIfNotSuccess(this RegisterUserResponse response)
        {
            if (response.Result == null)
            {
                var errors = response.GetErrors();
                throw new VCashException(errors);
            };
        }
    }
}
