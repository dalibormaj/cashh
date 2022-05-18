namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests
{
    public class RegisterUserRequest : HttpRequest
    {
        public string EmailVerificationUrl { get; set; }
        public string Email { get; set; }
        public PhoneContact[] PhoneContacts { get; set; }
        public ExtraRegistrationValues ExtraRegistrationValues { get; set; }
    }

    public class ExtraRegistrationValues
    {
        public bool ReceiveMarketingMessages { get; set; }
        public bool IsPoliticallyExposed { get; set; }
        public string CitizenId { get; set; }
    }

    public class PhoneContact
    {
        public string PhoneContactTypeCode { get; set; }
        public string Prefix { get; set; }
        public string Number { get; set; }
    }
}


