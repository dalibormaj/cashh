using MessagePack;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Infrastructure.Consumers.Abstraction;


namespace Victory.VCash.Infrastructure.Consumers.UserDetails
{
    public class UserDetailsConsumer : BaseConsumer
    {
        private IUnitOfWork _unitOfWork;
        private ILogger<UserDetailsConsumer> _logger;


        public UserDetailsConsumer(IModel channel, string queue, ILogger<UserDetailsConsumer> logger, IUnitOfWork unitOfWork) : base(channel, queue)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            //try
            //{
            //    var userRegisteredEvent = MessagePackSerializer.Deserialize<UserRegisteredEvent>(body);
            //    var promoCode = userRegisteredEvent?.ExtraProperties?
            //                                        .FirstOrDefault(x => "PromoCode".Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase))?
            //                                        .PropertyValue;

            //    //map the user according to the given data
            //    var user = new User()
            //    {
            //        UserId = userRegisteredEvent.UserId,
            //        LastName = userRegisteredEvent.LastName,
            //        Name = userRegisteredEvent.Name,
            //        UserName = userRegisteredEvent.UserName,
            //        UserTypeId = userRegisteredEvent.UserTypeCode?.ToUpper() switch
            //        {
            //            "PLYON" => UserType.PLYON,
            //            "AGENT" => UserType.AGENT,
            //            _ => UserType.PLYON
            //        }
            //    };

            //    if(user.UserTypeId == UserType.PLYON)
            //    {
            //        //Link user as a part of affiliate network
            //        var affiliateUser = _unitOfWork.GetRepository<UserRepository>().GetUserByAffiliateCode(promoCode);
            //        user.ParentUserId = affiliateUser?.UserId;
            //    }

            //    _unitOfWork.GetRepository<UserRepository>().SaveUser(user);
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogError(ex, $"Consumer {nameof(UserDetailsConsumer)} failed to consume data from queue '{Queue}' due to internal error", ex.Message);
            //}
            ////return base.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);
        }

        public override async Task HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
            //return base.HandleModelShutdown(model, reason);
        }
    }
}
