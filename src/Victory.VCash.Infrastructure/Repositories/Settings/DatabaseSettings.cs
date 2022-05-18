namespace Victory.VCash.Infrastructure.Repositories.Settings
{
    public class DatabaseSettings
    {
        public ConnectionStringsSection ConnectionStrings { get; set; }
        public RedisSection Redis { get; set; }
    }
}
