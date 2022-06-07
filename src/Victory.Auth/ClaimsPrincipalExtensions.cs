using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Victory.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetClaim(this IEnumerable<Claim> claims, string name, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return claims.SingleOrDefault(x => x.Type.Equals(name, stringComparison))?.Value;
        }

        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.Claims.GetClaim("UserId");
            if (string.IsNullOrEmpty(userId))
                return null;

            return Convert.ToInt32(userId);
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            //guardian claims
            var userName = user.Claims.GetClaim("UserName");

            //try azure ad claims
            if (string.IsNullOrEmpty(userName))
                userName = user.Claims.GetClaim("preferred_username");

            if (string.IsNullOrEmpty(userName))
                return null;

            return userName;
        }

        public static string GetName(this ClaimsPrincipal user)
        {
            return user.Claims.GetClaim("name");
        }

        public static DateTime? GetIssuedAt(this ClaimsPrincipal user)
        {
            string iat = user.Claims.GetClaim("iat");
            if (string.IsNullOrEmpty(iat))
                return null;

            return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(iat)).DateTime;
        }

        public static DateTime? GetExpiresAt(this ClaimsPrincipal user)
        {
            string exp = user.Claims.GetClaim("exp");
            if (string.IsNullOrEmpty(exp))
                return null;

            return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(exp)).DateTime;
        }

        public static bool IsAzureAdClaims(this ClaimsPrincipal user)
        {
            var userName = user.Claims.GetClaim("preferred_username");
            if (string.IsNullOrEmpty(userName))
                return false;

            return true;
        }
    }
}
