using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            return await response.ConvertToAsync<RegisterUserResponse>();
        }

        //public async Task<RegisterUserResponse> RegisterAgentAsync(RegisterAgentRequest request)
        //{
        //    var response = await _httpClient.PostAsync("/api/Security/Registration/RegisterNewUser", request.ToStringContent());
        //    return await response.ConvertToAsync<RegisterUserResponse>();
        //}

        public async Task<DefaultResponse> RequestPasswordResetAsync(RequestPasswordResetRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Accounts/RequestPasswordReset", request.ToStringContent());
            //try
            //{
                return await response.ConvertToAsync<DefaultResponse>();
            //}
            //catch
            //{
            //    var defaultResponse = response.ConvertTo<DefaultResponse>();
            //    throw new Exception($"{_httpClient.BaseAddress} responded with '{defaultResponse.Result?.ToList().FirstOrDefault() ?? defaultResponse.ResponseMessage}'");
            //}
        }

        public async Task<DefaultResponse> CompletePasswordResetAsync(CompletePasswordResetRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Accounts/CompletePasswordReset", request.ToStringContent());
            return await response.ConvertToAsync<DefaultResponse>();
        }
    }
}
