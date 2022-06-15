using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests
{
    public class SendSmsVerificationCodeRequest : HttpRequest
    {
        public string UserId { get; set; }
    }
}
