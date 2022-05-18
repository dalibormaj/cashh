using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests
{
    public class CompletePasswordResetRequest : HttpRequest
    {
        public string NewPassword { get; set; }
        public string ResetToken { get; set; }
    }
}
