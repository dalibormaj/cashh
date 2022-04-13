using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using StackExchange.Redis;
using System.Collections.Generic;
using Victory.DataAccess;
using Victory.Network.Infrastructure.Extensions;
using Victory.Network.Infrastructure.UnitOfWork.Settings;

namespace Victory.Network.Infrastructure.UnitOfWork
{
    public static class DataAccessExtensions
    {
        public static DatabaseSettings GetDatabaseSettings(this IConfiguration configuration)
        {
            DatabaseSettings settings = new DatabaseSettings();
            configuration.GetSection(typeof(DatabaseSettings).Name).Bind(settings);

            return settings;
        }

        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetDatabaseSettings();

            var redisOptions = new ConfigurationOptions();
            redisOptions.EndPoints.Add(dbSettings.Redis.Host, dbSettings.Redis.Port);
            redisOptions.ConnectTimeout = dbSettings.Redis.ConnectTimeout;
            redisOptions.SyncTimeout = dbSettings.Redis.SyncTimeout;
            redisOptions.AllowAdmin = dbSettings.Redis.AllowAdmin;
            redisOptions.ClientName = configuration.GetApplicationName();
            redisOptions.CommandMap = CommandMap.Create(new HashSet<string> { "SUBSCRIBE" }, false);//disables creating additional (pub/sub) connection

            services.AddSingleton<ICacheContext>(x => new CacheContext(redisOptions));
            services.AddTransient<ITestUnitOfWork>(x => ActivatorUtilities.CreateInstance<TestUnitOfWork>(x, new object[] { new DataContext<NpgsqlConnection>(dbSettings.ConnectionStrings.Postgre.Test) }));
        }
    }
}
