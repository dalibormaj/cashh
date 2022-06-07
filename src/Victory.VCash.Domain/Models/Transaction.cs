using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Models
{
    public class Transaction
    {
        public long? TransactionId { get; set; }
        public long? ExternalTransactionId { get; set; }
        public int? ExternalTransactionTypeId { get; set; }
        public long? ExternalGroupIdentifier { get; set; }
        public int? UserId { get; set; } 
        public decimal? Amount { get; set; } 
        public long? MoneyTransferId { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
