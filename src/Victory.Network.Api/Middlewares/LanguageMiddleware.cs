using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Victory.Network.Api.Middlewares
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LanguageMiddleware> _logger;

        public LanguageMiddleware(RequestDelegate next, ILogger<LanguageMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var contentLanguage = context.Request.Headers[HeaderNames.ContentLanguage].ToString();
            contentLanguage = string.IsNullOrEmpty(contentLanguage) ? "en-GB" : contentLanguage; //set default if empty

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(contentLanguage);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(contentLanguage);

            await _next.Invoke(context);
        }
    }
}
