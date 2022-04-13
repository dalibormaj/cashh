using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Victory.Network.Api.Dtos.Responses;
using Victory.Network.Infrastructure.Errors;
using Victory.Network.Infrastructure.Extensions;

namespace Victory.Network.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IOptions<JsonOptions> jsonOptions)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleGlobalExceptionAsync(httpContext, ex, jsonOptions);
            }
        }

        private async Task HandleGlobalExceptionAsync(HttpContext context, Exception exception, IOptions<JsonOptions> jsonOptions)
        {
            context.Response.ContentType = "application/json";
            BaseResponse response = new BaseResponse();
            if (exception is BaseException ex && ex.ErrorCodes != null)
            {
                foreach (var errorCode in ex.ErrorCodes)
                    response.Errors.Add(new ErrorResponse()
                    {
                        Code = errorCode,
                        Description = errorCode.GetDescription(true)
                    });
            }

            //if there is no error codes use exception message
            if (!(response?.Errors?.Any() ?? false))
            {
                response.Errors.Add(new ErrorResponse()
                {
                    Description = exception.Message
                });
            }
            
            var responseJson = JsonSerializer.Serialize(response, jsonOptions.Value.JsonSerializerOptions);

            if (exception is BaseException)
                //internal errors log as warnings
                _logger.LogWarning(exception, $"Response: {responseJson}");
            else
                _logger.LogError(exception, $"Response: {responseJson}");

            await context.Response.WriteAsync(responseJson);
        }
    }
}
