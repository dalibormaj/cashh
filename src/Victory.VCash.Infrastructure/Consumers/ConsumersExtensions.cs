using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using Victory.VCash.Infrastructure.Consumers.BetDetails;
using Victory.VCash.Infrastructure.Consumers.Settings;
using Victory.VCash.Infrastructure.Consumers.UserDetails;

namespace Victory.VCash.Infrastructure.Consumers
{
    public static class ConsumersExtensions
    {
        public static ConsumerSettings GetConsumerSettings(this IConfiguration configuration)
        {
            ConsumerSettings settings = new ConsumerSettings();
            configuration.GetSection(typeof(ConsumerSettings).Name).Bind(settings);

            return settings;
        }

        public static void AddConsumers(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetConsumerSettings();

            var connectionFactory = new ConnectionFactory
            {
                HostName = settings.RabbitMq.Host,
                Port = settings.RabbitMq.Port,
                VirtualHost = settings.RabbitMq.VirtualHost,
                UserName = settings.RabbitMq.UserName,
                Password = settings.RabbitMq.Password,
                ClientProvidedName = settings.RabbitMq.ClientProvidedName,
                DispatchConsumersAsync = true,
                ConsumerDispatchConcurrency = 20,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                RequestedHeartbeat = TimeSpan.FromSeconds(10),
            };
            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            services.AddTransient(x => ActivatorUtilities.CreateInstance<BetDetailsConsumer>(x, new object[] { channel, settings.RabbitMq.Queues.BetDetails }));
            services.AddTransient(x => ActivatorUtilities.CreateInstance<UserDetailsConsumer>(x, new object[] { channel, settings.RabbitMq.Queues.UserDetails }));
        }
    }
}
