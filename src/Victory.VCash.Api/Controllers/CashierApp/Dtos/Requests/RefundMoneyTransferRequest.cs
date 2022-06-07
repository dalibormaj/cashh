using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.CashierApp.Dtos.Requests
{
    public class RefundMoneyTransferRequest
    {
        [Required]
        public long TransactionId { get; set; }
    }
}
