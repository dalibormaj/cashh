using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Victory.VCash.Api.Controllers.Dtos.Responses
{
    public class BaseResponse
    {     
        public List<ErrorDto> Errors { get; init; } 
        public long Timestamp { get { return DateTimeOffset.UtcNow.ToUnixTimeSeconds(); } }
    }
}
