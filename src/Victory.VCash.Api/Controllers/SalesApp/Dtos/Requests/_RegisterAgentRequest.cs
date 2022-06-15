using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.SalesApp.Dtos.Requests
{
    public class _RegisterAgentRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string EmailVerificationUrl { get; set; }
        public bool ActivateAgent { get; set; } = false;
        [Required]
        public _CompanyDto Company { get; set; }
    }

    public class _CompanyDto
    {
        [Required]
        public string Name { get; set; }
        public string City { get; set; }
        public string Municipality { get; set; }
        public string Address { get; set; }
        public string GooglePlaceId { get; set; }
        public string GoogleFullAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string TaxNumber { get; set; }
        [Required]
        public string BankAccountNumber { get; set; }
        [Required]
        public List<_VenueDto> Venues { get; set; }
    }

    public class _VenueDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Municipality { get; set; }
        public string Address { get; set; }
        public string GooglePlaceId { get; set; }
        public string GoogleFullAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}


