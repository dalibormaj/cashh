using System;
using System.Linq;
using System.Security.Claims;

namespace Victory.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetId(this ClaimsPrincipal user)
        {
            var userId = user.Claims?.ToList().SingleOrDefault(x => "UserId".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            if (userId == null)
                return null;

            return Convert.ToInt32(userId);
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userName = user.Claims?.ToList().SingleOrDefault(x => "UserName".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            return userName;
        }
    }
}
