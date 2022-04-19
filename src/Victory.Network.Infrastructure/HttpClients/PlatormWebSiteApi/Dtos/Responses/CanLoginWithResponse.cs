namespace Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Responses
{
    public class CanLoginWithResponse : DefaultResponse<CanLoginWithResult>
    {
    }

    public class CanLoginWithResult
    {
        public int UserId { get; set; }
        public bool CanLogin { get; set; }
        public bool CanUsePassword { get; set; }
    }
}

