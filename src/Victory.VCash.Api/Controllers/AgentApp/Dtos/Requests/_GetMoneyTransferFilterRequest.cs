using System;
using Victory.VCash.Domain.Enums;

namespace Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests
{
    public class _GetMoneyTransferFilterRequest
    {
        public long? MoneyTransferId { get; set; }
        public int? FromUserId { get; set; }
        public int? ToUserId { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public MoneyTransferStatus? Status { get; set; }
    }
}
