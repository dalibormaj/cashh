using System;

namespace Victory.Network.Api.Dtos.Responses
{
    public class DepositResponse
    {
        public long TransactionId { get; set; }
        public string TypeCode { get; set; }
        public string StatusCode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
