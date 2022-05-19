using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.HttpClients.InternalApi;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests;
using Victory.VCash.Infrastructure.Repositories;


namespace Victory.VCash.Application.Services.UserService
{
    public class UserService : IUserService
    {
        IPlatformWebSiteApiClient _platformWebSiteApiClient;
        IInternalApiClient _internalApiClient;
        ILogger<UserService> _logger;
        IUnitOfWork _unitOfWork;

        public UserService(ILogger<UserService> logger, 
                           IPlatformWebSiteApiClient platformWebSiteApiClient, 
                           IInternalApiClient internalApiClient, 
                           IUnitOfWork unitOfWork)
        {
            _platformWebSiteApiClient = platformWebSiteApiClient;
            _internalApiClient = internalApiClient;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetAgentAsync(string identifier)
        {
            var user = await GetUserAsync(identifier);
            if (user?.UserTypeId.Value != UserType.AGENT)
                throw new VCashException(ErrorCode.AGENT_DOES_NOT_EXIST);

            return user;
        }

        public async Task<User> GetUserAsync(string identifier)
        {
            bool isIdentifierNumber = int.TryParse(identifier, out _);
            int? userId;

            if (isIdentifierNumber)
            {
                //if identifier is a number just parse to int
                userId = Convert.ToInt32(identifier);
            }
            else
            {
                //if it's a string ask platform for ID
                var canLoginResponse = await _platformWebSiteApiClient.CanLoginWithAsync(identifier);
                userId = canLoginResponse.Result.UserId;
            }

            if (userId == null)
                throw new VCashException(ErrorCode.USER_DOES_NOT_EXIST);

            var user = _unitOfWork.GetRepository<UserRepository>().GetUser(userId.Value);
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

            var response = await _platformWebSiteApiClient.RegisterUserAsync(request);

            if (response.ResponseCode != 0)
            {
                var errorMessage = string.IsNullOrEmpty(response.ResponseMessage) ? response.SystemMessage : response.ResponseMessage;
                throw new VCashException(errorMessage);
            }

            var userId = Convert.ToInt32(response.Result.UserId);
            var user = new User()
            {
                UserId = userId
            };
            _unitOfWork.GetRepository<UserRepository>().SaveUser(user);

            return userId;
        }

        public async Task RequestPasswordResetAsync(string userIdentifier, string passwordResetUrl)
        {
            var response = await _platformWebSiteApiClient.RequestPasswordResetAsync(new RequestPasswordResetRequest() 
            { 
                UserName = userIdentifier,
                PasswordResetPageUrl = passwordResetUrl 
            });

            if (response.ResponseCode != 0)
                throw new VCashException(ErrorCode.AGENT_DOES_NOT_EXIST);
        }

        public async Task CompletePasswordResetAsync(string newPassword, string token)
        {
            var response = await _platformWebSiteApiClient.CompletePasswordResetAsync(new CompletePasswordResetRequest() { NewPassword = newPassword, ResetToken = token });
            if (response.ResponseCode != 0)
                throw new VCashException(ErrorCode.PASSWORD_RESET_CANNOT_COMPLETE);
        }
    }
}
