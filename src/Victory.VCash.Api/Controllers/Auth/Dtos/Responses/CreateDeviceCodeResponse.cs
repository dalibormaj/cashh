using System;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;

namespace Victory.VCash.Api.Controllers.Auth.Dtos.Responses
{
    public class CreateDeviceCodeResponse : BaseResponse
    {
        public string DeviceCode { get; set; }
        public int ExpiresInMin { get; set; }
    }
}
