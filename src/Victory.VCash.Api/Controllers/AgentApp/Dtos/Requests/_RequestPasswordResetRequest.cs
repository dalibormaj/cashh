using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests
{
    public class _RequestPasswordResetRequest
    {
        [Required]
        public string UserIdentifier { get; set; }
        [Required]
        public string PasswordResetUrl { get; set; }
    }
}
