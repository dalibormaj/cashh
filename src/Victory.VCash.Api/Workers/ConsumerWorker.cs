using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.Consumers;
using Victory.VCash.Infrastructure.Consumers.BetDetails;
using Victory.VCash.Infrastructure.Consumers.UserDetails;

namespace Victory.VCash.Api.Workers
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly ILogger<ConsumerWorker> _logger;
        private readonly BetDetailsConsumer _betConsumer;
        private readonly UserDetailsConsumer _userConsumer;

        public ConsumerWorker(ILogger<ConsumerWorker> logger, 
                              BetDetailsConsumer betConsumer, 
                              UserDetailsConsumer userConsumer)
        {
            _betConsumer = betConsumer;
            _userConsumer = userConsumer;   
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //start consuming
            //_betConsumer.Start();
            _userConsumer.Start();

            await Task.CompletedTask;

            //consumer.Received += async (sender, eventArgs) =>
            //{
            //    RegulatoryQueueMessage message = new();
            //    var success = true;
            //    try
            //    {
            //        var isMessageTypeExist = eventArgs.BasicProperties.Headers?.ContainsKey("type") ?? false;
            //        var messageType = isMessageTypeExist ? Encoding.UTF8.GetString((byte[])eventArgs.BasicProperties.Headers["type"]).ToLower() : string.Empty;
            //        message = PrepareMessage(messageType, eventArgs.Body.ToArray());
            //        _logger.LogInformation("Message processing, Message data: {@message}", message);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, "An error occured while parsing the message, Message data: {@message}", message);
            //    }

            //    if (success)
            //    {
            //        _channel.BasicAck(eventArgs.DeliveryTag, false);
            //    }
            //    else if (eventArgs.Redelivered)
            //    {
            //        _channel.BasicNack(eventArgs.DeliveryTag, false, false);
            //        _logger.LogWarning("Message processing not successful after redilivery. Skipping ACK, Message data: {@message}", message);
            //    }
            //    else
            //    {
            //        await Task.Delay(_nackDelay);
            //        _channel.BasicNack(eventArgs.DeliveryTag, false, true);
            //        _logger.LogWarning("Message processing not successful. Redelivering.  Message data: {@message}", message);
            //    }
            //};

            //consumer.Shutdown += async (sender, eventArgs) => {
            //    if (cancellationToken.IsCancellationRequested)
            //    {
            //        _logger.LogInformation("Consumer shutdown (cancellation requested) {@eventArgs}", eventArgs);
            //    }
            //    else
            //    {
            //        _logger.LogError("Consumer shutdown {@eventArgs}", eventArgs);
            //    }
            //    await Task.CompletedTask;
            //};
        }

        //private RegulatoryQueueMessage PrepareMessage(string messageType, byte[] messageBody)
        //{
        //    var message = new RegulatoryQueueMessage();
        //    _logger.LogDebug($"Received message: {MessagePackSerializer.ConvertToJson(messageBody)}");
        //    switch (messageType)
        //    {
        //        case "cancel":
        //            message.Type = MessageType.BettingData;
        //            var cancelData = MessagePackSerializer.Deserialize<RegulatoryReportBetDetailQueueMessage>(messageBody);
        //            cancelData.Status = cancelData.Status == BetStatus.Placed ? BetStatus.Cancelled : cancelData.Status;
        //            message.Data = cancelData;
        //            break;

        //        case "settle":
        //            message.Type = MessageType.BettingData;
        //            var settleData = MessagePackSerializer.Deserialize<RegulatoryReportBetDetailQueueMessage>(messageBody);
        //            settleData.Status = settleData.Status == BetStatus.Placed ? BetStatus.Won : settleData.Status;
        //            message.Data = settleData;
        //            break;

        //        case "bet":
        //            message.Type = MessageType.BettingData;
        //            message.Data = MessagePackSerializer.Deserialize<RegulatoryReportBetDetailQueueMessage>(messageBody);
        //            break;

        //        case "financial":
        //            message.Type = MessageType.FinancialTransaction;
        //            message.Data = MessagePackSerializer.Deserialize<RegulatoryReportTransactionDetailQueueMessage>(messageBody);
        //            break;

        //        default:
        //            throw new NotSupportedException($"Message type not recognized: {messageType}");
        //    }
        //    return message;
        //}

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
