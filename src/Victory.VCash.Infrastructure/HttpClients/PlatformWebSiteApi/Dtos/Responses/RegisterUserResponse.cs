using System.Collections.Generic;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses
{
    public class RegisterUserResponse : BaseResponse<RegisterUserResult>
    {
        public override List<Error> GetErrors()
        {
            if (Result != null)
                return null;

            var errors = new List<Error>();
            var errorMessage = string.IsNullOrEmpty(ResponseMessage) ? null : $"{ResponseMessage}";
            errors.Add(ResponseCode switch
            {
                2 => new Error(ErrorCode.BAD_REQUEST, $"{errorMessage} [{SystemMessage?.Replace("'", "")}]"),
                20100 => new Error(ErrorCode.AGENT_REGISTRATION_FAILED),
                20214 => new Error(ErrorCode.MOBILE_NUMBER_ALREADY_IN_USE),
                20217 => new Error(ErrorCode.INVALID_MOBILE_NUMBER),
                20812 => new Error(ErrorCode.INVALID_EMAIL),
                20831 => new Error(ErrorCode.PASSWORD_DOES_NOT_MEET_COMPLEXITY_REQUIREMENTS),
                _ => new Error(ErrorCode.SYSTEM_ERROR, errorMessage)
            });

            return errors;
        }
    }

    public class RegisterUserResult
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
    }
}



