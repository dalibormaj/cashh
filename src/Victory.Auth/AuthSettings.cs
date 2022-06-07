namespace Victory.Auth
{
    public class AuthSettings
    {
        public GuardianAuthSettings Guardian { get; set; }
        public AzureAdAuthSettings AzureAd { get; set; }
        public VCashAuthSettings VCash { get; set; }
    }

    public class GuardianAuthSettings
    {
        public string Url { get; set; }
    }

    public class AzureAdAuthSettings
    {
        public string OpenIdConfigUrl { get; set; }
    }

    public class VCashAuthSettings
    {
        public string Url { get; set; }
        public string CashierTokenKey { get; set; }
    }
}
