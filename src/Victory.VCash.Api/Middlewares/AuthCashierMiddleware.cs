using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Application.Services.CashierService;
using Victory.VCash.Infrastructure.Repositories;

namespace Victory.VCash.Api.Middlewares
{
    public class AuthCashierMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthCashierMiddleware> _logger;
        private readonly IAgentService _agentService;
        private readonly ICashierService _cashierService;

        public AuthCashierMiddleware(RequestDelegate next, 
                                     ILogger<AuthCashierMiddleware> logger, 
                                     IAgentService agentService, 
                                     ICashierService cashierService)
        {
            _logger = logger;
            _next = next;
            _agentService = agentService;
            _cashierService = cashierService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var accessToken = context.Request.Headers["X-CASHIER"].ToString();
            if (!string.IsNullOrEmpty(accessToken))
            {
                var validationResult = _cashierService.ValidateAccessToken(accessToken);
                if(!validationResult.IsValid)
                    throw new UnauthorizedAccessException();

                context.User.AddIdentity(new ClaimsIdentity(validationResult.Claims));
            }

            await _next.Invoke(context);
        }
    }
}
