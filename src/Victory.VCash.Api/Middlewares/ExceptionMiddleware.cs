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
using Victory.VCash.Api.Controllers.Dtos;
using Victory.VCash.Api.Controllers.Dtos.Responses;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.Extensions;

namespace Victory.VCash.Api.Middlewares
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

            //CONTENT
            BaseResponse response = new BaseResponse() { Errors = new List<ErrorDto>() };
            if (exception is BaseException ex && ex?.Errors != null)
            {
                foreach (var error in ex.Errors)
                    response.Errors.Add(new ErrorDto()
                    {
                        Code = error.ErrorCode,
                        Description = string.Format(error.ErrorCode.GetDescription(true), error.Args)
                    });
            }

            //if there is no error codes use exception message
            if (!(response?.Errors?.Any() ?? false))
            {
                response.Errors.Add(new ErrorDto()
                {
                    Code = ErrorCode.SYSTEM_ERROR,
                    Description = string.Format(ErrorCode.SYSTEM_ERROR.GetDescription(true), exception.Message)
                });
            }
            
            var responseJson = JsonSerializer.Serialize(response, jsonOptions.Value.JsonSerializerOptions);

            //STATUS CODE
            if(exception is UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }


            //LOG
            if (exception is BaseException ||
                exception is UnauthorizedAccessException)
                _logger.LogWarning(exception, $"Response: {responseJson}");
            else
                _logger.LogError(exception, $"Response: {responseJson}");

            await context.Response.WriteAsync(responseJson);
        }
    }
}
