using Collector.Serilog.Enrichers.SensitiveInformation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;
using System;

namespace Victory.Network
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.AddSerilog(new LoggerConfiguration().ReadFrom
                                  .Configuration(hostingContext.Configuration)
                                  .Filter.ByExcluding(x => (x.Exception?.GetType().Namespace.Contains("Microsoft") ?? false) && x.Level <= LogEventLevel.Warning) //Ignore Microsoft Info/Warning logs
                                  .WriteTo.RabbitMQ(ConfigureRabbitMqSink(hostingContext.Configuration)) //TODO: remove after sink bug is fixed: https://github.com/steffenlyng/serilog-sinks-rabbitmq/issues/119
                                  .Enrich.With(new SensitiveInformationEnricher())
                                  .CreateLogger());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static Action<RabbitMQClientConfiguration, RabbitMQSinkConfiguration> ConfigureRabbitMqSink(IConfiguration config)
        {
            return (c, s) =>
            {
                const string clientConfig = "Serilog:WriteTo:0:Args:ClientConfiguration";
                const string sinkConfig = "Serilog:WriteTo:0:Args:SinkConfiguration";

                config.GetSection(clientConfig).Bind(c);
                config.GetSection(sinkConfig).Bind(s);
                s.TextFormatter = new CompactJsonFormatter();
            };
        }
    }
}
