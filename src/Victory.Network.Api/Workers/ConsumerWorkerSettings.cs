namespace Victory.Network.Api.Workers
{
    public class ConsumerWorkerSettings
    {
        public string VirtualHost { get; set; }

        public string Host { get; set; }

        public ushort Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ClientProvidedName { get; set; }

        public string Queue { get; set; }

        public string ExchangeName { get; set; }

        public string RoutingKey { get; set; }
    }
}
