namespace Victory.Network.Infrastructure.UnitOfWork.Settings
{
    public class DatabaseSettings
    {
        public ConnectionStringsSection ConnectionStrings { get; set; }
        public RedisSection Redis { get; set; }
    }
}
