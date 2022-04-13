namespace Victory.Network.Api.Dtos.Requests
{
    public class UserWithdrawRequest
    {
        public int UserId { get; set; } 
        public decimal Amount { get; set; }
    }
}
