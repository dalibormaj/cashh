using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses
{
    public class SendSmsVerificationCodeResponse : BaseResponse<SendSmsVerificationCodeResult>
    {
        public override List<Error> GetErrors()
        {
            if (Result != null)
                return null;

            var errors = new List<Error>();
            var errorMessage = string.IsNullOrEmpty(ResponseMessage) ? null : $"{ResponseMessage}";
            errors.Add(ResponseCode switch
            {
                _ => new Error(ErrorCode.SYSTEM_ERROR, errorMessage)
            });
            return errors;
        }
    }

    public class SendSmsVerificationCodeResult
    {
        public string UserId { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
