using StackExchange.Redis;
using System;

namespace Victory.DataAccess
{
    public class CacheContext : ICacheContext
    {
        private Lazy<IConnectionMultiplexer> lazyConnection;
        public CacheContext(ConfigurationOptions options)
        {
            lazyConnection = lazyConnection?? new Lazy<IConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(options);
            });
        }

        public IConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
