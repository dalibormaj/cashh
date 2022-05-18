using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Requests
{
    public class GetTransactionsRequest : HttpRequest
    {
        public int UserId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
