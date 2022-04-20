using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Victory.Network.Infrastructure.Extensions;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Requests;
using Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi.Dtos.Responses;

namespace Victory.Network.Infrastructure.HttpClients.PlatormWebSiteApi
{
    public class PlatormWebSiteApiClient : IPlatformWebSiteApiClient
    {
        private readonly HttpClient _httpClient;

        public PlatormWebSiteApiClient(ILogger<PlatormWebSiteApiClient> logger, HttpClient httpClient)
        {
            _httpClient = httpClient;

            if (_httpClient == null) throw new InvalidOperationException($"{GetType().Name} cannot be created");


            var language = Thread.CurrentThread.CurrentUICulture.Name;
            var LANGUAGE_HEADER_NAME = "X-API-LANGUAGE";
            if (!string.IsNullOrEmpty(language) && !_httpClient.DefaultRequestHeaders.Contains(LANGUAGE_HEADER_NAME))
                _httpClient.DefaultRequestHeaders.Add(LANGUAGE_HEADER_NAME, language);
        }

        public async Task<CanLoginWithResponse> CanLoginWithAsync(CanLoginWithRequest request)
        {
            var response = await _httpClient.GetAsync($"/api/Security/Accounts/CanLoginWith?userName={request.UserName}");
            return await response.ConvertToAsync<CanLoginWithResponse>();
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            var response = await _httpClient.PostAsync("/api/Security/Registration/RegisterNewUser", request.ToStringContent());
            return await response.ConvertToAsync<RegisterUserResponse>();
        }
    }
}
