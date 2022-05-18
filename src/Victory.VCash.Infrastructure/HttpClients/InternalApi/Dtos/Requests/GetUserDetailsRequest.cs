using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Requests
{
    public class GetUserDetailsRequest : HttpRequest
    {
        public string TpsUserId { get; set; }
    }
}
