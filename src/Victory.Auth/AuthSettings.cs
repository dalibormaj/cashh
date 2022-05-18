namespace Victory.Auth
{
    public class AuthSettings
    {
        public GuardianSettings Guardian { get; set; }
        public AzureAdSettings AzureAd { get; set; }

    }

    public class GuardianSettings
    {
        public string Url { get; set; }
    }

    public class AzureAdSettings
    {
        public string OpenIdConfigUrl { get; set; }
    }
}
