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
    public class PlatormWebSiteApiClient : IPlatormWebSiteApiClient
    {
        private readonly HttpClient _httpClient;

        public PlatormWebSiteApiClient(ILogger<IPlatormWebSiteApiClient> logger, HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            if (_httpClient == null) throw new InvalidOperationException("PlatormWebSiteApiClient cannot be created");
            var headers = GetDefaultHeaders();

            var response = await _httpClient.PostAsync("/api/Security/Registration/RegisterNewUser", request.ToStringContent(headers));
            return await response.ConvertToAsync<RegisterUserResponse>();
        }

        private Dictionary<string, string> GetDefaultHeaders()
        {
            var language = Thread.CurrentThread.CurrentUICulture.Name;

            var headers = new Dictionary<string, string>();
            headers.Add("X-API-LANGUAGE", language);

            return headers;
        }
    }
}
