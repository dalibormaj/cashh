using StackExchange.Redis;

namespace Victory.DataAccess
{
    public interface ICacheContext
    {
        IConnectionMultiplexer Connection { get; }
    }
}
