namespace Victory.Network.Infrastructure.Repositories.Settings
{
    public class RedisSection
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int ConnectTimeout { get; set; }
        public int SyncTimeout { get; set; }
        public bool AllowAdmin { get; set; }
    }
}
