using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.Auth.Dtos.Requests
{
    public class CreateDeviceCodeRequest
    {
        [Required]
        public string AgentId { get; set; }
        public string DeviceName { get; set; }
    }
}
