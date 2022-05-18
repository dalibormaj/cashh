namespace Victory.VCash.Api.AdminControllers.Dtos.Requests
{
    public class _RegisterAgentRequest
    {
        public string CitizenId { get; set; }
        public string EmailVerificationUrl { get; set; }
        public string Email { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public string CompanyRegistrationNumber { get; set; } 
        public string CompanyTaxNumber { get; set; }
    }
}


