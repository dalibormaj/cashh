namespace Victory.VCash.Infrastructure.Consumers.Settings
{ 
    public class ConsumerSettings
    {
        public bool Enable { get; set; }
        public RabbitMqSection RabbitMq { get; set; }
    }

    public class RabbitMqSection
    {
        public string VirtualHost { get; set; }

        public string Host { get; set; }

        public ushort Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ClientProvidedName { get; set; }

        public QueueSection Queues { get; set; }

        public string ExchangeName { get; set; }

        public string RoutingKey { get; set; }
    }

    public class QueueSection
    {
        public string BetDetails { get; set; }
        public string UserDetails { get; set; }
    }
}
