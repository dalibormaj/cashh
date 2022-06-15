using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.Extensions;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Responses;

namespace Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi
{
    public class PlatformWebSiteApiClient : IPlatformWebSiteApiClient
    {
        private readonly HttpClient _httpClient;

        public PlatformWebSiteApiClient(ILogger<PlatformWebSiteApiClient> logger, HttpClient httpClient)
        {
            _httpClient = httpClient;

            if (_httpClient == null) throw new InvalidOperationException($"{GetType().Name} cannot be created");


            var language = Thread.CurrentThread.CurrentUICulture.Name;
            var LANGUAGE_HEADER_NAME = "X-API-LANGUAGE";
            if (!string.IsNullOrEmpty(language) && !_httpClient.DefaultRequestHeaders.Contains(LANGUAGE_HEADER_NAME))
                _httpClient.DefaultRequestHeaders.Add(LANGUAGE_HEADER_NAME, language);
        }

        public async Task<CanLoginWithResponse> CanLoginWithAsync(string userName)
        {
            var response = await _httpClient.GetAsync($"/api/Security/Accounts/CanLoginWith?userName={userName}");
            return await response.ConvertToAsync<CanLoginWithResponse>();
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Registration/RegisterNewUser", request.ToStringContent());
            try
            {
                return await response.ConvertToAsync<RegisterUserResponse>();
            }
            catch
            {
                var defaultResponse = response.ConvertTo<DefaultResponse>();
                return new RegisterUserResponse()
                {
                    ResponseCode = defaultResponse.ResponseCode,
                    ResponseMessage = defaultResponse.ResponseMessage,
                    SystemMessage = defaultResponse.SystemMessage ?? defaultResponse.Result.FirstOrDefault()
                };
            }
    }

        public async Task<DefaultResponse> RequestPasswordResetAsync(RequestPasswordResetRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Accounts/RequestPasswordReset", request.ToStringContent());
            return await response.ConvertToAsync<DefaultResponse>();
        }

        public async Task<DefaultResponse> CompletePasswordResetAsync(CompletePasswordResetRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Accounts/CompletePasswordReset", request.ToStringContent());
            return await response.ConvertToAsync<DefaultResponse>();
        }

        public async Task<OverridePasswordResponse> OverridePasswordAsync(OverridePasswordRequest request, string authToken)
        {
            var headers = new Dictionary<string, string>()
            {
                {
                    HeaderNames.Authorization,
                    authToken
                }
            };

            var response = await _httpClient.PostAsync("/api/Security/Accounts/OverridePassword", request.ToStringContent(headers));
            return await response.ConvertToAsync<OverridePasswordResponse>();
        }

        public async Task<SendEmailVerificationCodeResponse> SendEmailVerificationCodeAsync(SendEmailVerificationCodeRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Registration/SendEmailVerificationCode", request.ToStringContent());
            return await response.ConvertToAsync<SendEmailVerificationCodeResponse>();
        }

        public async Task<SendSmsVerificationCodeResponse> SendSmsVerificationCodeAsync(SendSmsVerificationCodeRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Registration/SendSmsVerificationCode", request.ToStringContent());
            return await response.ConvertToAsync<SendSmsVerificationCodeResponse>();
        }

        public async Task<VerifyEmailResponse> VerifyEmailAsync(VerifyEmailRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Registration/VerifyEmail", request.ToStringContent());
            try
            {
                return await response.ConvertToAsync<VerifyEmailResponse>();
            }
            catch
            {
                var defaultResponse = response.ConvertTo<DefaultResponse>();
                return new VerifyEmailResponse()
                {
                    ResponseCode = defaultResponse.ResponseCode,
                    ResponseMessage = defaultResponse.ResponseMessage,
                    SystemMessage = defaultResponse.SystemMessage ?? defaultResponse.Result.FirstOrDefault()
                };
            }
        }

        public async Task<VerifySmsResponse> VerifySmsAsync(VerifySmsRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Registration/VerifyMobileNumber", request.ToStringContent());
            try
            {
                return await response.ConvertToAsync<VerifySmsResponse>();
            }
            catch
            {
                var defaultResponse = response.ConvertTo<DefaultResponse>();
                return new VerifySmsResponse()
                {
                    ResponseCode = defaultResponse.ResponseCode,
                    ResponseMessage = defaultResponse.ResponseMessage,
                    SystemMessage = defaultResponse.SystemMessage ?? defaultResponse.Result.FirstOrDefault()
                };
            }
        }
    }
}
