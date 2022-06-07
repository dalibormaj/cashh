using System;

namespace Victory.VCash.Api.Controllers.Auth.Dtos.Responses
{
    public class ValidateDeviceTokenResponse : BaseResponse
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string AgentId { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
