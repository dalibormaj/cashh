using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.Auth.Dtos.Requests
{
    public class CreateDeviceTokenRequest
    {
        [Required]
        public string AgentId { get; set; }
        [Required]
        public string DeviceCode { get; set; }
    }
}
