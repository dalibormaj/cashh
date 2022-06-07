using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Api.Middlewares
{
    public class UnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnauthorizedMiddleware> _logger;

        public UnauthorizedMiddleware(RequestDelegate next, ILogger<UnauthorizedMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IOptions<JsonOptions> jsonOptions)
        {
            await _next(httpContext);

            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized &&
                httpContext.Items.ContainsKey(AuthContextConstants.AUTH_FAILED_REASONS))
            {
                var errors = (Dictionary<string, string>)httpContext.Items[AuthContextConstants.AUTH_FAILED_REASONS];

                BaseResponse response = new BaseResponse() { Errors = new List<ErrorDto>() };
                foreach(var error in errors)
                {
                    ErrorCode errorCode;
                    if(!Enum.TryParse(error.Key, out errorCode))
                        errorCode = ErrorCode.AUTHORIZATION_FAILED;

                    response.Errors.Add(new ErrorDto()
                    {
                        Code = errorCode,
                        Description = error.Value
                    });
                }

                var responseJson = JsonSerializer.Serialize(response, jsonOptions.Value.JsonSerializerOptions);
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(responseJson);
            }
        }
    }
}
