using System;

namespace Victory.VCash.Api.Controllers.CashierApp.Dtos
{
    public class TransactionDto
    {
        public long TransactionId { get; set; }
        public long ExternalTransactionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
    }
}
