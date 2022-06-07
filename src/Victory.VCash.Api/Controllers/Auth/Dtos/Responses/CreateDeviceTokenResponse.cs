using System;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;

namespace Victory.VCash.Api.Controllers.Auth.Dtos.Responses
{
    public class CreateDeviceTokenResponse : BaseResponse
    {
        public string Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
