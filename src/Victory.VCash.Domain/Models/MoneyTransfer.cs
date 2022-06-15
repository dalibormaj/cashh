using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Enums;

namespace Victory.VCash.Domain.Models
{
    public class MoneyTransfer : BaseDomainModel
    {
        public long? MoneyTransferId { get; init; }
        public int? FromUserId { get; set; }
        public int? ToUserId { get; set; }   
        public decimal? Amount { get; set; }
        public MoneyTransferStatus? MoneyTransferStatusId { get; set; }
        public string Error { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}