using System;

namespace Victory.Network.Api.Dtos.Responses
{
    public class DepositResponse : BaseResponse
    {
        public long TransactionId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
