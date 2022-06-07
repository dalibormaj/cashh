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
            CurrentDevice agent = null;
            CurrentCashier cashier = null;
            CurrentAdmin admin = null;

            var claims = context.User.Claims;

            var deviceId = claims.GetClaim("device_id");
            var deviceName = claims.GetClaim("device_name");
            var agentId = claims.GetClaim("agent_id");
            var agentUserId = claims.GetClaim("agent_user_id");
            var cashierId = claims.GetClaim("cashier_id");
            var cashierUserName = claims.GetClaim("cashier_user_name");

            if (!string.IsNullOrEmpty(deviceId))
            {
                agent = new CurrentDevice()
                {
                    DeviceId = Convert.ToInt32(deviceId),
                    DeviceName = deviceName,
                    AgentId = agentId,
                    AgentUserId = Convert.ToInt32(agentUserId)
                };
            }

            if (!string.IsNullOrEmpty(cashierId))
            {
                cashier = new CurrentCashier()
                {
                    CashierId = cashierId,
                    UserName = cashierUserName
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
                Device = agent,
                Admin = admin
            };
        }
    }

    internal class Current
    {
        public CurrentCashier Cashier { get; init; }
        public CurrentDevice Device { get; init; }
        public CurrentAdmin Admin { get; init; }
    }

    internal class CurrentCashier
    {
        public string CashierId { get; init; }
        public string UserName { get; init; }
    }

    internal class CurrentDevice
    {
        public int DeviceId { get; init; }
        public string DeviceName { get; init; }
        public string AgentId { get; init; }
        public int AgentUserId { get; init; }
    }

    internal class CurrentAdmin
    {
        public string UserName { get; init; }
        public string Name { get; init; }
    }
}
