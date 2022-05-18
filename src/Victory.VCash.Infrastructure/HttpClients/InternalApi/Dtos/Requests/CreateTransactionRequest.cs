using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Requests
{
    public class CreateTransactionRequest : HttpRequest
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public int TxTypeId { get; set; }
        public string ExternalTransactionId { get; set; }
        public long? GroupIdentifier { get; set; }
        public bool CapToBalance { get; set; }
        public Dictionary<string, string> ExtraDetails { get; set; }
    }
}
