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
using Victory.Network.Infrastructure.Extensions;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Requests;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.Network.Infrastructure.HttpClients.InternalApi
{
    public class InternalApiClient : IInternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _saltKey;
        private const string SECYRITY_HASH_HEADER_NAME = "securityhash";
        private const string LANGUAGE_HEADER_NAME = "X-API-LANGUAGE";

        private string ComputeSecurityHash<T>(T request, string saltKey) where T: HttpRequest
        {
            string clearString = $"{saltKey}{Regex.Replace(request.ToJson(), @"\s+", "")}";
            using var sha512 = SHA512.Create();
            byte[] hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(clearString));
            return Convert.ToBase64String(hashedBytes);
        }

        public InternalApiClient(ILogger<InternalApiClient> logger, HttpClient httpClient, InternalApiSettings settings)
        {
            _httpClient = httpClient;
            _saltKey = settings.SaltKey;

            if (_httpClient == null) throw new InvalidOperationException($"{GetType().Name} cannot be created");

            var language = Thread.CurrentThread.CurrentUICulture.Name;
            if (!string.IsNullOrEmpty(language) && !_httpClient.DefaultRequestHeaders.Contains(LANGUAGE_HEADER_NAME))
                _httpClient.DefaultRequestHeaders.Add(LANGUAGE_HEADER_NAME, language);
        }

        public async Task<GetUserDetailsResponse> GetUserDetails(GetUserDetailsRequest request)
        {
            var headers = new Dictionary<string, string>()
            {
                {
                    SECYRITY_HASH_HEADER_NAME,
                    ComputeSecurityHash(request, _saltKey)
                }
            };

            var response = await _httpClient.PostAsync("/api/userDetails", request.ToStringContent(headers));
            return await response.ConvertToAsync<GetUserDetailsResponse>();
        }

        public async Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request)
        {
            var headers = new Dictionary<string, string>()
            {
                {
                    SECYRITY_HASH_HEADER_NAME,
                    ComputeSecurityHash(request, _saltKey)
                }
            };
            var response = await _httpClient.PostAsync("api/finance/financialTransaction/create", request.ToStringContent(headers));
            return await response.ConvertToAsync<CreateTransactionResponse>();
        }
    }
}
