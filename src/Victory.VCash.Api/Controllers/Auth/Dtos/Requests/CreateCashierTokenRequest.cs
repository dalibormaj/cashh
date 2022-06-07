using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.Auth.Dtos.Requests
{
    public class CreateCashierTokenRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Pin { get; set; }
    }
}
