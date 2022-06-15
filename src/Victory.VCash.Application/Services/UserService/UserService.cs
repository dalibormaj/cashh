using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Application.Services.UserService.Outputs;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Common;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.HttpClients.InternalApi;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Requests;
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

        public async Task<GetUserOutput> GetUserAsync(string identifier, bool maskBasicValues = false)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new VCashException(ErrorCode.PL_USER_CANNOT_BE_FOUND);

            bool isIdentifierNumber = long.TryParse(identifier, out _);
            bool isPhoneNumber = identifier.StartsWith("+") || 
                                 (isIdentifierNumber && Convert.ToInt64(identifier) > 60000000 && Convert.ToInt64(identifier) < 700000000) ||
                                 (identifier.StartsWith("381") && isIdentifierNumber && Convert.ToInt64(identifier) > 381600000);
            bool isIndetierUserId = isIdentifierNumber && !isPhoneNumber;
            int? userId;

            if (isIndetierUserId)
            {
                //if identifier is a userId just parse to int
                userId = Convert.ToInt32(identifier);
            }
            else
            {
                //if it's a string ask platform for ID
                var canLoginResponse = await _platformWebSiteApiClient.CanLoginWithAsync(identifier);
                userId = canLoginResponse.Result.UserId;
            }

            if (userId == null)
                throw new VCashException(ErrorCode.PL_USER_CANNOT_BE_FOUND);

            var user = await _internalApiClient.GetUserDetailsAsync(new GetUserDetailsRequest() { TpsUserId = userId.ToString() });
            if (user == null)
                throw new VCashException(ErrorCode.PL_USER_CANNOT_BE_FOUND);

            var output = new GetUserOutput()
            {
                UserId = Convert.ToInt32(user.UserId),
                UserTypeCode = user.UserType.Code,
                UserName = user.Username,
                Email = user.Emails?.FirstOrDefault().Email,
                Name = user.UserDetail?.Name,
                LastName = user.UserDetail?.Surname,
                MobilePhoneNumber = user.PhoneContactDetails?.FirstOrDefault()?.ContactNumber,
                CitizenId = user.ExtraDetails?
                                .FirstOrDefault(x => "Blinking.CitizenId".Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase))
                                .PropertyValue,
                StatusCode = user.UserStatus.Code,
                BirthDate = user.UserDetail.BirthDate == default? null : user.UserDetail.BirthDate
            };

            if (maskBasicValues) 
            {
                output.UserName = (identifier.Equals(output.UserName)) ? output.UserName : output.UserName.MaskRight("***");
                output.MobilePhoneNumber = isPhoneNumber ? output.MobilePhoneNumber : output.MobilePhoneNumber.MaskRight("***");
                output.CitizenId = (identifier.Equals(output.CitizenId)) ? output.CitizenId : output.CitizenId.MaskRight("***");
            };

            return output;
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
            return userId;
        }
    }
}
