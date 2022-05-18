using System;
using Victory.VCash.Domain.Enums;

namespace Victory.VCash.Api.Controllers.Dtos.Requests
{
    public class GetMoneyTransferFilterRequest
    {
        public long? MoneyTransferId { get; set; }
        public int? ToUserId { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public MoneyTransferStatus? Status { get; set; }
    }
}
