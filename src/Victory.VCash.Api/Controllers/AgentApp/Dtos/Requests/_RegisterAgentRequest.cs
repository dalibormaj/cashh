using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests
{
    public class _RegisterAgentRequest
    {
        [Required]
        public string EmailVerificationUrl { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public _CompanyDto Company { get; set; }
    }

    public class _CompanyDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Municipality { get; set; }
        public string Address { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxNumber { get; set; }
        public List<_VenueDto> Venues { get; set; }
    }

    public class _VenueDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Municipality { get; set; }
        public string Address { get; set; }
    }
}


