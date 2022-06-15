using Microsoft.AspNetCore.Http;
using System;
using Victory.Auth;

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
            CurrentDevice device = null;
            CurrentCashier cashier = null;
            CurrentAzureAd azureAd = null;
            CurrentGuardian guardian = null;

            var claims = context.User.Claims;

            var deviceId = claims.GetClaim("device_id");
            var deviceName = claims.GetClaim("device_name");
            var agentId = claims.GetClaim("agent_id");
            var agentUserId = claims.GetClaim("agent_user_id");
            var cashierId = claims.GetClaim("cashier_id");
            var cashierUserName = claims.GetClaim("cashier_user_name");
            var userId = claims.GetClaim("user_id");
            var userName = claims.GetClaim("user_name");
            var guardianToken = claims.GetClaim("guardian_token");

            if (!string.IsNullOrEmpty(deviceId))
            {
                device = new CurrentDevice()
                {
                    DeviceId = Convert.ToInt32(deviceId),
                    DeviceName = deviceName,
                    AgentId = string.IsNullOrEmpty(agentId)? null : new Guid(agentId),
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
                azureAd = new CurrentAzureAd()
                {
                    UserName = context.User.GetUserName(),
                    Name = context.User.GetName()
                };
            }

            if (context.User.IsGuardianClaims())
            {
                int _userId;
                int.TryParse(userId, out _userId);

                guardian = new CurrentGuardian()
                {
                    UserId = _userId,
                    UserName = userName,
                    AccessToken = guardianToken
                };
            }

            return new Current()
            {
                Cashier = cashier,
                Device = device,
                AzureAd = azureAd,
                Guardian = guardian
            };
        }
    }

    internal class Current
    {
        public CurrentCashier Cashier { get; init; }
        public CurrentDevice Device { get; init; }
        public CurrentAzureAd AzureAd { get; init; }
        public CurrentGuardian Guardian { get; init; }
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
        public Guid? AgentId { get; init; }
        public int AgentUserId { get; init; }
    }

    internal class CurrentAzureAd
    {
        public string UserName { get; init; }
        public string Name { get; init; }
    }

    internal class CurrentGuardian
    {
        public int UserId { get; init; }
        public string UserName { get; init; }
        public string AccessToken { get; init; }
    }
}
