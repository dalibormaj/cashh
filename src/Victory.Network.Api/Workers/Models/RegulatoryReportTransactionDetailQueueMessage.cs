using MessagePack;

namespace Victory.Network.Api.Workers.Models
{
    [MessagePackObject]
    public class RegulatoryReportTransactionDetailQueueMessage
    {
        [Key("TransactionId")]
        public long TransactionId { get; set; }

        [Key("IsRollback")]
        public bool IsRollback { get; set; }
    }
}
