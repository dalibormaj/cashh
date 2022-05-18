using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Responses
{
    public class GetTransactionsReponse
    {
        public List<TpsTransaction> Transactions { get; set; }
    }

    public class TpsTransaction
    {
		public long TransactionId { get; set; }
		public long TxGroupId { get; set; }
		public int TxTypeId { get; set; }
		public string TxTypeCode { get; set; }
		public int TxStatusId { get; set; }
		public string TxStatusCode { get; set; }
		public int UserAccountId { get; set; }
		public string UserAccountTypeCode { get; set; }
		public int UserId { get; set; }
		public decimal Amount { get; set; }
		public decimal? BalanceAfterTx { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }
		public string CreatedByIpAddress { get; set; }
	}
}
