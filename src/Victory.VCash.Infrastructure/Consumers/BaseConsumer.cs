using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Victory.VCash.Infrastructure.Consumers.Abstraction
{
    public abstract class BaseConsumer : AsyncEventingBasicConsumer
    {
        private readonly IModel _channel;
        private readonly string _consumerTag;
        public string Queue { get; private init; }


        protected BaseConsumer(IModel channel, string queue) : base(channel)
        {
            _channel = channel;
            Queue = queue;
            _consumerTag = Guid.NewGuid().ToString();

        }

        public void Start()
        {
            _channel.BasicConsume(Queue, false, _consumerTag, this);
        }

        public void Stop()
        {
            if (_channel == null)
                throw new Exception("Consumer has not started yet");

            _channel.BasicCancel(_consumerTag);
        }
    }
}
