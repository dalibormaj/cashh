using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Responses
{
    public abstract class DefaultResponse<T>
    {
        public T Result { get; set; }
        public string SystemMessage { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
