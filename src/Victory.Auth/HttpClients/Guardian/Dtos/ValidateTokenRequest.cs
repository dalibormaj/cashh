namespace Victory.Auth.HttpClients.Guardian.Dtos
{
    public class ValidateTokenRequest
    {
        public string Token { get; set; }
        public string ApiId { get; set; }
        public string ApiSecret { get; set; }
    }
}
