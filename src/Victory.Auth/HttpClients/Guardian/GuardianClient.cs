using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Victory.Auth.HttpClients.Guardian.Models;

namespace Victory.Auth.HttpClients.Guardian
{
    public class GuardianClient : IGuardianClient 
    {
        private readonly HttpClient _httpClient;

        public GuardianClient(ILogger<GuardianClient> logger, HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ValidateTokenResponse> ValidateTokenAsync(string token)
        {
            if (_httpClient == null) throw new InvalidOperationException("GuardianClient cannot be created");

            var request = new ValidateTokenRequest()
            {
                Token = token
            };

            var jsonRequest = JsonSerializer.Serialize(request);

            var response = await _httpClient.PostAsync("/auth/validate", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

            var jsonResponse = await response.Content?.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ValidateTokenResponse>(jsonResponse);
        }
    }
}
