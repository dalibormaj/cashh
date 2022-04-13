namespace Victory.Network.Infrastructure.Repositories.Models
{
    public class UserExtraDetail
    {
        public int UserExtraDetailId { get; set; }
        public int UserDetailId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }
}
