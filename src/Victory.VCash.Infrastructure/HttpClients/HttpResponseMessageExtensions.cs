using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients
{
    public static class HttpResponseMessageExtensions
    {
        public static T ConvertTo<T>(this HttpResponseMessage response)
        {
            var jsonResponse = response.Content?.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(jsonResponse);
        }

        public async static Task<T> ConvertToAsync<T>(this HttpResponseMessage response, bool returnDefaultIfFails = false)
        {
            var jsonResponse = await response.Content?.ReadAsStringAsync();
            try 
            { 
                return JsonSerializer.Deserialize<T>(jsonResponse);
            }
            catch(Exception ex)
            {
                if(returnDefaultIfFails) 
                    return default(T);
                throw new Exception($"Failed while deserializing the response message", ex);
            }
        }
    }
}
