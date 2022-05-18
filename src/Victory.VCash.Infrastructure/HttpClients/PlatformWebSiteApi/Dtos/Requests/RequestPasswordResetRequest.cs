using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests
{
    public class RequestPasswordResetRequest :HttpRequest
    {
        public string UserName { get; set; }
        public string PasswordResetPageUrl { get; set; }
    }
}
