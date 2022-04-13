using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Victory.Auth.HttpClients.Guardian.Models
{
    public class ValidateTokenResponse
    {
        public bool IsValidated { get; set; }
        public int ValidationResult { get; set; }
        public string ValidationError { get; set; }
        [JsonPropertyName("id")]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public Dictionary<string, string> ServiceTokenData { get; set; }
    }
}
