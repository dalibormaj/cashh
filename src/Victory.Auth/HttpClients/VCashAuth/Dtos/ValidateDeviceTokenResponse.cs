using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Victory.Auth.HttpClients.VCashAuth.Dtos
{
    public class ValidateDeviceTokenResponse
    {
        [JsonPropertyName("errors")]
        public ErrorDto[] Errors { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }

    public class ErrorDto
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}



