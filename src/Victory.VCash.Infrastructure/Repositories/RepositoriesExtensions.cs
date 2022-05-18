using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using StackExchange.Redis;
using System.Collections.Generic;
using Victory.DataAccess;
using Victory.VCash.Infrastructure.Extensions;
using Victory.VCash.Infrastructure.Repositories.Abstraction;
using Victory.VCash.Infrastructure.Repositories.Settings;

namespace Victory.VCash.Infrastructure.Repositories
{
    internal static class RepositoriesExtensions
    {
        public static DatabaseSettings GetDatabaseSettings(this IConfiguration configuration)
        {
            DatabaseSettings settings = new DatabaseSettings();
            configuration.GetSection(typeof(DatabaseSettings).Name).Bind(settings);

            return settings;
        }

        internal static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
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
            services.AddTransient<IDataContext>(x => new DataContext<NpgsqlConnection>(dbSettings.ConnectionStrings.Postgres.VictoryNetwork));
            services.AddTransient<IUnitOfWork, DefaultUnitOfWork>();

        }
    }
}
