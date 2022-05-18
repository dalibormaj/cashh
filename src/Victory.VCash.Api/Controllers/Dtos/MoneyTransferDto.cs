using System;
using System.Collections.Generic;

namespace Victory.VCash.Api.Controllers.Dtos
{
    public class MoneyTransferDto
    {
        public long MoneyTransferId { get; set; }
        public int? FromUserId { get; set; }
        public int? ToUserId { get; set; }
        public decimal? Amount { get; set; }
        public string StatusCode { get; set; }
        public DateTime? Date { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}
