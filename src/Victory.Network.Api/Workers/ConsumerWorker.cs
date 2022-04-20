using MessagePack;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Victory.Network.Api.Workers.Models;

namespace Victory.Network.Api.Workers
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly ILogger<ConsumerWorker> _logger;
        private readonly TimeSpan _nackDelay;
        private readonly IModel _channel;
        private readonly IConnection _connection;

        public ConsumerWorker(ILogger<ConsumerWorker> logger, IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _nackDelay = TimeSpan.FromSeconds(10);
            _connection = connectionFactory.CreateConnection() ?? throw new InvalidOperationException("Unable to create connection to RabbitMq");
            _channel = _connection.CreateModel() ?? throw new InvalidOperationException("Unable to create channel for publishing.");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (s, e) =>
            {
                RegulatoryQueueMessage message = new();
                var success = true;
                try
                {
                    message = PrepareMessage(Encoding.UTF8.GetString((byte[])e.BasicProperties.Headers["type"])?.ToLower(), e.Body.ToArray());
                    _logger.LogInformation("Message processing, Message data: {@message}", message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured while parsing the message, Message data: {@message}", message);
                }

                if (success)
                {
                    _channel.BasicAck(e.DeliveryTag, false);
                }
                else if (e.Redelivered)
                {
                    _channel.BasicNack(e.DeliveryTag, false, false);
                    _logger.LogWarning("Message processing not successful after redilivery. Skipping ACK, Message data: {@message}", message);
                }
                else
                {
                    await Task.Delay(_nackDelay);
                    _channel.BasicNack(e.DeliveryTag, false, true);
                    _logger.LogWarning("Message processing not successful. Redelivering.  Message data: {@message}", message);
                }
            };

            consumer.Shutdown += async (s, e) => {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Consumer shutdown (cancellation requested) {@e}", e);
                }
                else
                {
                    _logger.LogError("Consumer shutdown {@e}", e);
                    //Run(cancellationToken, onMessageCallbackAsync);
                }
                await Task.CompletedTask;
            };

            _channel.BasicConsume("platform.production.default.betdetails.vnet.queue", false, consumer);

            await Task.CompletedTask;
        }

        private RegulatoryQueueMessage PrepareMessage(string messageType, byte[] messageBody)
        {
            var message = new RegulatoryQueueMessage();
            _logger.LogDebug($"Received message: {MessagePackSerializer.ConvertToJson(messageBody)}");
            switch (messageType)
            {
                case "cancel":
                    message.Type = MessageType.BettingData;
                    var cancelData = MessagePackSerializer.Deserialize<RegulatoryReportBetDetailQueueMessage>(messageBody);
                    cancelData.Status = cancelData.Status == BetStatus.Placed ? BetStatus.Cancelled : cancelData.Status;
                    message.Data = cancelData;
                    break;

                case "settle":
                    message.Type = MessageType.BettingData;
                    var settleData = MessagePackSerializer.Deserialize<RegulatoryReportBetDetailQueueMessage>(messageBody);
                    settleData.Status = settleData.Status == BetStatus.Placed ? BetStatus.Won : settleData.Status;
                    message.Data = settleData;
                    break;

                case "bet":
                    message.Type = MessageType.BettingData;
                    message.Data = MessagePackSerializer.Deserialize<RegulatoryReportBetDetailQueueMessage>(messageBody);
                    break;

                case "financial":
                    message.Type = MessageType.FinancialTransaction;
                    message.Data = MessagePackSerializer.Deserialize<RegulatoryReportTransactionDetailQueueMessage>(messageBody);
                    break;

                default:
                    throw new NotSupportedException($"Message type not recognized: {messageType}");
            }
            return message;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
