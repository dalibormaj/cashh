using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace Victory.Network.Api.Workers
{
    public static class ConsumerWorkerExtensions
    {
        public static ConsumerWorkerSettings GetConsumerWorkerSettings(this IConfiguration configuration)
        {
            ConsumerWorkerSettings settings = new ConsumerWorkerSettings();
            configuration.GetSection(typeof(ConsumerWorkerSettings).Name).Bind(settings);

            return settings;
        }

        public static void AddConsumerWorker(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetConsumerWorkerSettings();
            services.AddHostedService<ConsumerWorker>();
            services.AddTransient((Func<IServiceProvider, IConnectionFactory>)(p =>
            {
                return new ConnectionFactory
                {
                    HostName = settings.Host,
                    Port = settings.Port,
                    VirtualHost = settings.VirtualHost,
                    UserName = settings.UserName,
                    Password = settings.Password,
                    ClientProvidedName = settings.ClientProvidedName,
                    DispatchConsumersAsync = true,
                    ConsumerDispatchConcurrency = 20,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                    RequestedHeartbeat = TimeSpan.FromSeconds(10),
                };
            }));
        }
    }
}
