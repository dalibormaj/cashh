using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Responses
{
    public class CreateTransactionResponse
    {
        public long TransactionId { get; set; }
        public decimal EffectiveTransactionAmount { get; set; }
        public long GroupIdentifier { get; set; }
        public DateTime TransactionTimeStamp { get; set; }
        public bool SuccessCall { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}