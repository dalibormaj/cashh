namespace Victory.VCash.Api.Controllers.Dtos.Requests
{
    public class CreateCashierAccessTokenRequest
    {
        public string UserName { get; set; }
        public string Pin { get; set; }
    }
}
