using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.Network.Domain.Models;
using Victory.Network.Infrastructure.Errors;
using Victory.Network.Infrastructure.HttpClients.InternalApi;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Requests;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Responses;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Requests;
using Victory.Network.Infrastructure.Repositories;


namespace Victory.Network.Application.Services.UserService
{
    public class UserService : IUserService
    {
        IPlatformWebSiteApiClient _platormWebSiteApiClient;
        IInternalApiClient _internalApiClient;
        ILogger<UserService> _logger;
        IUnitOfWork _unitOfWork;

        public UserService(ILogger<UserService> logger, 
                           IPlatformWebSiteApiClient platormWebSiteApiClient, 
                           IInternalApiClient internalApiClient, 
                           IUnitOfWork unitOfWork)
        {
            _platormWebSiteApiClient = platormWebSiteApiClient;
            _internalApiClient = internalApiClient;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetUserDetailsResponse> GetUser(string identifier)
        {
            var user =  await _internalApiClient.GetUserDetails(new GetUserDetailsRequest() { TpsUserId = identifier });
            return user;
        }

        public async Task<int> RegisterUserAsync (int registedByUserId,
                                                                 string citizenId, 
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
            var user = new User()
            {
                UserId = userId,
                ParentId = registedByUserId
            };
            await _unitOfWork.GetRepository<UserRepository>().SaveUser(user);

            return userId;
        }
    }
}
