namespace Victory.Network.Infrastructure.Repositories.Settings
{
    public class ConnectionStringsSection
    {
        public SqlServerSection SqlServer { get; set; }
        public PostgreSection Postgre { get; set; }
    }
}