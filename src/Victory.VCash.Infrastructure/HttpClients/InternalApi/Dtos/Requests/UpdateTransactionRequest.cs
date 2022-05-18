using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Requests
{
    public class UpdateTransactionRequest : HttpRequest
    {
        public long TransactionIdentifier { get; set; }
        public int TransactionTypeId { get; set; }
        public int NewTransactionStatusId { get; set; }
        public string IpAddress { get; set; }
        public Dictionary<string, string> ExtraDetails { get; set; }
    }
}