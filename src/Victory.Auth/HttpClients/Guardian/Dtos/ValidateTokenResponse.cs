using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Victory.Auth.HttpClients.Guardian.Dtos
{
    public class ValidateTokenResponse
    {
        [JsonPropertyName("isValidated")]
        public bool IsValidated { get; set; }
        [JsonPropertyName("validationResult")]
        public int ValidationResult { get; set; }
        [JsonPropertyName("validationError")]
        public string ValidationError { get; set; }
        [JsonPropertyName("id")]
        public string UserId { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
        [JsonPropertyName("serviceTokenData")]
        public Dictionary<string, string> ServiceTokenData { get; set; }
    }
}
