using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests
{
    public class _RegisterCashierRequest
    {
        [Required]
        public int VenueId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pin { get; set; }
    }
}