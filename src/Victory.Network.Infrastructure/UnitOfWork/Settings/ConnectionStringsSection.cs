namespace Victory.Network.Infrastructure.UnitOfWork.Settings
{
    public class ConnectionStringsSection
    {
        public SqlServerSection SqlServer { get; set; }
        public PostgreSection Postgre { get; set; }
    }
}