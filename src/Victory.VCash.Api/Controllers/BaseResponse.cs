using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Api.Controllers
{
    public class BaseResponse
    {     
        public List<ErrorDto> Errors { get; init; } 
        public long Timestamp { get { return DateTimeOffset.UtcNow.ToUnixTimeSeconds(); } }
    }

    public class ErrorDto
    {
        public ErrorCode? Code { get; set; }
        public string Description { get; set; }
    }
}
