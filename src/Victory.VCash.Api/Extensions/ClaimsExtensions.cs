using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Victory.Auth;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Api.Extensions
{

    /// <summary>
    /// Extension is used to group all claims by the user type
    /// 
    /// </summary>
    internal static class ClaimsExtensions
    {
        public static Current Current(this HttpContext context)
        {
            CurrentAgent agent = null;
            CurrentCashier cashier = null;
            CurrentAdmin admin = null;

            var agentId = context.User.Claims?.ToList().SingleOrDefault(x => "agent_id".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            if (!string.IsNullOrEmpty(agentId))
            {
                agent = new CurrentAgent()
                {
                    AgentId = agentId,
                    UserId = context.User.GetUserId().Value,
                    UserName = context.User.GetUserName()
                };
            }

            var cashierUserName = context.User.Claims?.ToList().SingleOrDefault(x => "cashier_user_name".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            var cashierId = context.User.Claims?.ToList().SingleOrDefault(x => "cashier_id".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            var parentAgentId = context.User.Claims?.ToList().SingleOrDefault(x => "cashier_parent_agent_id".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            var parentAgentUserId = context.User.Claims?.ToList().SingleOrDefault(x => "cashier_parent_agent_user_id".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            if (!string.IsNullOrEmpty(cashierUserName))
            {
                cashier = new CurrentCashier()
                {
                    CashierId = cashierId,
                    UserName = cashierUserName,
                    ParentAgentId = parentAgentId,
                    ParentAgentUserId = string.IsNullOrEmpty(parentAgentUserId)? null : Convert.ToInt32(parentAgentUserId)
                };
            }

            if (context.User.IsAzureAdClaims())
            {
                admin = new CurrentAdmin()
                {
                    UserName = context.User.GetUserName(),
                    Name = context.User.GetName()
                };
            }

            return new Current()
            {
                Cashier = cashier,
                Agent = agent,
                Admin = admin
            };
        }
    }

    internal class Current
    {
        public CurrentCashier Cashier { get; init; }
        public CurrentAgent Agent { get; init; }
        public CurrentAdmin Admin { get; init; }

        public void ValidateCashier()
        {
            if(Cashier == null)
                throw new VCashBadRequestException(ErrorCode.BAD_REQUEST, "Missing X-CASHIER header value");

            if (Agent == null)
                throw new VCashBadRequestException(ErrorCode.BAD_REQUEST, "Missing Authorization header value");

            if (Cashier?.ParentAgentUserId != null &&
               Agent?.UserId != null &&
               Cashier.ParentAgentUserId != Agent.UserId)
            {
                throw new VCashException(ErrorCode.CASHIER_IS_NOT_IN_THE_AGENT_NETWORK);
            }
        }
    }

    internal class CurrentCashier
    {
        public string CashierId { get; init; }
        public string ParentAgentId { get; init; }
        public int? ParentAgentUserId { get; init; }
        public string UserName { get; init; }
    }

    internal class CurrentAgent
    {
        public string AgentId { get; init; }
        public int? UserId { get; init; }
        public string UserName { get; init; }
    }

    internal class CurrentAdmin
    {
        public string UserName { get; init; }
        public string Name { get; init; }
    }
}
