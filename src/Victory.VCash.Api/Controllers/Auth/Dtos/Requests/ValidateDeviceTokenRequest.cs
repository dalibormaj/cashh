using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.Auth.Dtos.Requests
{
    public class ValidateDeviceTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
