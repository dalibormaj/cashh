using MessagePack;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Infrastructure.Consumers.Abstraction;

namespace Victory.VCash.Infrastructure.Consumers.BetDetails
{
    public class BetDetailsConsumer : BaseConsumer
    {
        private IUnitOfWork _unitOfWork;
        private ILogger<BetDetailsConsumer> _logger;
        public BetDetailsConsumer(IModel channel, string queue, ILogger<BetDetailsConsumer> logger, IUnitOfWork unitOfWork) : base(channel, queue)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public override Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            MessagePackSerializer.Deserialize<string>(body);
            return Task.CompletedTask;  
        }

        public override Task HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
            return base.HandleModelShutdown(model, reason);
        }
    }
}
