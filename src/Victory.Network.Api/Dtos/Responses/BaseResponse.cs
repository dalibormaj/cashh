using System;
using System.Collections.Generic;

namespace Victory.Network.Api.Dtos.Responses
{
    public class BaseResponse
    {
        public List<ErrorResponse> Errors { get; set; } = new List<ErrorResponse>();
        public long Timestamp { get { return DateTimeOffset.UtcNow.ToUnixTimeSeconds(); } }
    }
}
