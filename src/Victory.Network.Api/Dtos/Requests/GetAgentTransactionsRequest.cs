using System;

namespace Victory.Network.Api.Dtos.Requests
{
    public class GetAgentTransactionsRequest
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
