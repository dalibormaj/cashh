using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Victory.Network.Application.Services.UserService.Outputs;
using Victory.Network.Infrastructure.Errors;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Requests;

namespace Victory.Network.Application.Services.UserService
{
    public class UserService : IUserService
    {
        IPlatormWebSiteApiClient _platormWebSiteApiClient;
        ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger, IPlatormWebSiteApiClient platormWebSiteApiClient)
        {
            _platormWebSiteApiClient = platormWebSiteApiClient;
            _logger = logger;
        }

        public async Task<RegisterUserOutput> RegisterUserAsync (string citizenId, 
                                                                 string emailVerificationUrl, 
                                                                 string email,
                                                                 string mobilePhoneNumber,
                                                                 bool canReceiveMarketingMessages,
                                                                 bool IsPoliticallyExposed)
        {
            var request = new RegisterUserRequest()
            {
                ExtraRegistrationValues = new ExtraRegistrationValues()
                {
                    CitizenId = citizenId,
                    IsPoliticallyExposed = IsPoliticallyExposed,
                    ReceiveMarketingMessages = canReceiveMarketingMessages
                },
                Email = email,
                EmailVerificationUrl = emailVerificationUrl,
                PhoneContacts = new[]{ new PhoneContact()
                {
                    PhoneContactTypeCode = "MB",
                    Prefix = "+381",
                    Number = mobilePhoneNumber
                }}
            };

            var response = await _platormWebSiteApiClient.RegisterUserAsync(request);

            if (response.ResponseCode != 0)
            {
                var errorMessage = string.IsNullOrEmpty(response.ResponseMessage) ? response.SystemMessage : response.ResponseMessage;
                throw new VictoryNetworkException(errorMessage);
            }

            var userId = Convert.ToInt32(response.Result.UserId);
            return new RegisterUserOutput(userId);
        }
    }
}
