using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Victory.Network.Infrastructure.HttpClients
{
    public abstract class HttpRequest
    {
        public StringContent ToStringContent(Dictionary<string, string> headers)
        {
            var jsonRequest = ToJson();
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            //add header values
            if(headers != null)
                foreach(var header in headers)
                {
                    content.Headers.Add(header.Key, header.Value);
                }       

            return content;
        }

        public StringContent ToStringContent()
        {
            return ToStringContent(null);
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, GetType());
        }
    }
}
