using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.DataAccess;
using Victory.VCash.Application.Services.AgentService;
using Victory.VCash.Infrastructure.Repositories;

namespace Victory.VCash.Api.Middlewares
{
    public class AuthAgentMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthAgentMiddleware> _logger;
        private readonly IAgentService _agentService;

        public AuthAgentMiddleware(RequestDelegate next, ILogger<AuthAgentMiddleware> logger, IAgentService agentService)
        {
            _logger = logger;
            _next = next;
            _agentService = agentService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var agentUserId = context.User.GetUserId();//get the platform userId that agent is using for logging in

            if (agentUserId != null)
            {
                //check if user id is a valid agent
                var agent = _agentService.GetAgent(agentUserId.Value);
                if (agent != null)
                {
                    context.User.AddIdentity(new ClaimsIdentity(new[] { new Claim("agent_id", agent.AgentId.ToString()) }));
                }
            }

            await _next.Invoke(context);
        }
    }
}
