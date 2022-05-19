using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Victory.VCash.Api.Middlewares
{
    public class OperatorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<OperatorMiddleware> _logger;

        public OperatorMiddleware(RequestDelegate next, ILogger<OperatorMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers[HeaderNames.Authorization].ToString();
            if (!string.IsNullOrEmpty(token))
            {
                const string TOKEN_DECORATOR = "#vcash-op:";
                int endOfTokenPosition = token.Length;

                if (token.Contains(TOKEN_DECORATOR))
                {
                    endOfTokenPosition = token.IndexOf(TOKEN_DECORATOR);
                    var operatorValues = token.Substring(token.IndexOf(TOKEN_DECORATOR) + TOKEN_DECORATOR.Length);
                    
                    var operatorUserName = operatorValues.Substring(0, operatorValues.IndexOf(":"));
                    var operatorPin = operatorValues.Substring(operatorValues.IndexOf(":"))?.Replace(":","");

                    //TODO!!!
                    //check here if PIN is valid
                    var isPinValid = true;
                    if (!isPinValid)
                        throw new UnauthorizedAccessException();

                    context.Items.Add("OperatorUserName", operatorUserName);
                }

                //remove decorated values
                token = token.Substring(0, endOfTokenPosition);
                context.Request.Headers[HeaderNames.Authorization] = token;
            }
            await _next.Invoke(context);
        }
    }
}
