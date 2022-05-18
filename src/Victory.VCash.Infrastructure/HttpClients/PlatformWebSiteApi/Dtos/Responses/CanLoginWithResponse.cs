namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses
{
    public class CanLoginWithResponse : BaseResponse<CanLoginWithResult>
    {
    }

    public class CanLoginWithResult
    {
        public int UserId { get; set; }
        public bool CanLogin { get; set; }
        public bool CanUsePassword { get; set; }
    }
}

