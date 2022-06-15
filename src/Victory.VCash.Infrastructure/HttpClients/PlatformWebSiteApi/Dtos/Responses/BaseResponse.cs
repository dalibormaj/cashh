using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses
{
    public abstract class BaseResponse<T>
    {
        public T Result { get; set; }
        public string SystemMessage { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }

        //override with mapped values in order to get valid error codes
        public virtual List<Error> GetErrors()
        {
            return null;
        }

        public void ThrowIfNotSuccess()
        {
            if (Result == null || ResponseCode > 0)
            {
                var errors = GetErrors();
                throw new VCashException(errors);
            };
        }
    }
}
