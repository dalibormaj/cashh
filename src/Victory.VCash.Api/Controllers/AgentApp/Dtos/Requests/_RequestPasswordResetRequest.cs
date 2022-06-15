using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests
{
    public class _OverridePasswordRequest
    {
        [Required]
        public string NewPassword { get; set; }
    }
}
