using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses
{
    public class VerifyEmailResponse : BaseResponse<VerifyEmailResult>
    {
        public override List<Error> GetErrors()
        {
            if (Result != null && Result.IsSuccessful)
                return null;

            var errors = new List<Error>();
            var errorMessage = string.IsNullOrEmpty(ResponseMessage) ? null : $"{ResponseMessage}";
            errors.Add(ResponseCode switch
            {
                20820 => new Error(ErrorCode.INVALID_VERIFICATION_CODE),
                _ => new Error(ErrorCode.SYSTEM_ERROR, errorMessage)
            });
            return errors;
        }
    }

    public class VerifyEmailResult
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
