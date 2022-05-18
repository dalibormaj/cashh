//using MessagePack;
//using System;

//namespace Victory.VCash.Api.Workers.Models
//{
//    [MessagePackObject]
//    public class RegulatoryReportBetDetailQueueMessage
//    {
//        [Key("CouponCode")]
//        public string CouponCode { get; set; }

//        [Key("UserId")]
//        public int UserId { get; set; }

//        [Key("BetStatus")]
//        public string BetStatusName { get; set; }

//        [IgnoreMember]
//        public BetStatus Status
//        {
//            get => Enum.Parse<BetStatus>(BetStatusName);
//            set => BetStatusName = Enum.GetName(value);
//        }

//        [Key("CitizenId")]
//        public string CitizenId { get; set; }

//        [Key("TransactionGroupId")]
//        public long TransactionGroupId { get; set; }
//    }
//}
