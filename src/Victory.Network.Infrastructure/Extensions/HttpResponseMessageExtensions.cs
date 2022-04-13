using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Victory.Network.Infrastructure.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static T ConvertTo<T>(this HttpResponseMessage response)
        {
            var jsonResponse = response.Content?.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(jsonResponse);
        }

        public async static Task<T> ConvertToAsync<T>(this HttpResponseMessage response)
        {
            var jsonResponse = await response.Content?.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(jsonResponse);          
        }
    }
}
