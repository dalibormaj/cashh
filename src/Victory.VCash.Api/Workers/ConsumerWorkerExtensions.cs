using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using Victory.VCash.Infrastructure.Consumers;
using Victory.VCash.Infrastructure.Consumers.Settings;

namespace Victory.VCash.Api.Workers
{
    public static class ConsumerWorkerExtensions
    {
        public static void AddVictoryWorkers(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetConsumerSettings();

            if (settings.Enabled)
            {
                //mq consumers
                services.AddConsumers(configuration);

                //add background services for consuming data
                services.AddHostedService<ConsumerWorker>();
            }
        }
    }
}
