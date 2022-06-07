using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Victory.Auth.HttpClients.Guardian.Dtos;
using Victory.Auth.HttpClients.VCashAuth.Dtos;

namespace Victory.Auth.HttpClients.Guardian
{
    public class VCashAuthClient : IVCashAuthClient
    {
        private readonly HttpClient _httpClient;

        public VCashAuthClient(ILogger<VCashAuthClient> logger, HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ValidateDeviceTokenResponse> ValidateDeviceTokenAsync(string token)
        {
            if (_httpClient == null) throw new InvalidOperationException("VCashAuthClient cannot be created");

            try
            {
                var request = new ValidateDeviceTokenRequest()
                {
                    Token = token
                };

                var jsonRequest = JsonSerializer.Serialize(request);
                var response = await _httpClient.PostAsync("auth/device/validate", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
                var jsonResponse = await response.Content?.ReadAsStringAsync();
            
                return JsonSerializer.Deserialize<ValidateDeviceTokenResponse>(jsonResponse);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Device token authorization failed", ex);
            };
        }
    }
}
