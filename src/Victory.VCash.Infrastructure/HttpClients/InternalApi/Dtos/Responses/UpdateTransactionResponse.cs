using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Responses
{
    public class UpdateTransactionResponse
    {
        public long TransactionId { get; set; }
        public bool SuccessCall { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}