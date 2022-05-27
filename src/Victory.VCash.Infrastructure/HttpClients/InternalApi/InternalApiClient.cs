using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.Extensions;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Requests;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi
{
    public class InternalApiClient : IInternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _saltKey;
        private const string SECYRITY_HASH_HEADER_NAME = "securityhash";
        private const string LANGUAGE_HEADER_NAME = "X-API-LANGUAGE";

        public InternalApiClient(ILogger<InternalApiClient> logger, HttpClient httpClient, InternalApiSettings settings)
        {
            _httpClient = httpClient;
            _saltKey = settings.SaltKey;

            if (_httpClient == null) throw new InvalidOperationException($"{GetType().Name} cannot be created");

            var language = Thread.CurrentThread.CurrentUICulture.Name;
            if (!string.IsNullOrEmpty(language) && !_httpClient.DefaultRequestHeaders.Contains(LANGUAGE_HEADER_NAME))
                _httpClient.DefaultRequestHeaders.Add(LANGUAGE_HEADER_NAME, language);
        }

        private string ComputeSecurityHash<T>(T request, string saltKey) where T : HttpRequest
        {
            string clearString = $"{saltKey}{Regex.Replace(request.ToJson(), @"\s+", "")}";
            using var sha512 = SHA512.Create();
            byte[] hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(clearString));
            return Convert.ToBase64String(hashedBytes);
        }

        public async Task<GetUserDetailsResponse> GetUserDetailsAsync(GetUserDetailsRequest request)
        {
            var headers = new Dictionary<string, string>()
            {
                {
                    SECYRITY_HASH_HEADER_NAME,
                    ComputeSecurityHash(request, _saltKey)
                }
            };

            var endpoint = "api/userDetails";
            var response = await _httpClient.PostAsync(endpoint, request.ToStringContent(headers));
            var jsonResponse = await response.Content?.ReadAsStringAsync();

            if (jsonResponse?.Contains("User was not found", StringComparison.OrdinalIgnoreCase) ?? false)
                return null;

            return await response.ConvertToAsync<GetUserDetailsResponse>();
        }

        public async Task<CreateTransactionResponse> CreateTransactionAsync(CreateTransactionRequest request)
        {
            var headers = new Dictionary<string, string>()
            {
                {
                    SECYRITY_HASH_HEADER_NAME,
                    ComputeSecurityHash(request, _saltKey)
                }
            };

            var endpoint = "api/finance/financialTransaction/create";
            var response = await _httpClient.PostAsync(endpoint, request.ToStringContent(headers));
            if(!response.IsSuccessStatusCode)
                throw new Exception($"Failed to invoke request to {_httpClient.BaseAddress}{endpoint}");

            return await response.ConvertToAsync<CreateTransactionResponse>();
        }

        public async Task<UpdateTransactionResponse> UpdateTransactionAsync(UpdateTransactionRequest request)
        {
            var headers = new Dictionary<string, string>()
            {
                {
                    SECYRITY_HASH_HEADER_NAME,
                    ComputeSecurityHash(request, _saltKey)
                }
            };

            var endpoint = "api/finance/financialTransaction/updateStatus";
            var response = await _httpClient.PostAsync(endpoint, request.ToStringContent(headers));

            return await response.ConvertToAsync<UpdateTransactionResponse>();
        }

        public async Task<GetTransactionsReponse> GetTransactionsAsync(GetTransactionsRequest request)
        {
            var headers = new Dictionary<string, string>()
            {
                {
                    SECYRITY_HASH_HEADER_NAME,
                    ComputeSecurityHash(request, _saltKey)
                }
            };
            var query = new Dictionary<string, string>()
            {
                [nameof(request.UserId)] = request.UserId.ToString(),
                [nameof(request.DateFrom)] = request.DateFrom.ToShortDateString(),
                [nameof(request.DateTo)] = request.DateTo.Value.ToShortDateString()
            };

            var endpoint = "api/finance/financialTransaction";
            var uri = QueryHelpers.AddQueryString(endpoint, query);
            var response = await _httpClient.GetAsync(uri);

            return await response.ConvertToAsync<GetTransactionsReponse>();
        }
    }
}
