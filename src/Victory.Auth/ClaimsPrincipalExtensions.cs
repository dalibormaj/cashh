using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Victory.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.Claims?.ToList().SingleOrDefault(x => "UserId".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;

            return Convert.ToInt32(userId);
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            //guardian claims
            var userName = user.Claims?.ToList().SingleOrDefault(x => "UserName".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;

            //try azure ad claims
            if(string.IsNullOrEmpty(userName))
                userName = user.Claims?.ToList().SingleOrDefault(x => "preferred_username".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;

            if (string.IsNullOrEmpty(userName))
                return null;

            return userName;
        }

        public static string GetName(this ClaimsPrincipal user)
        {
            var name = user.Claims?.ToList().SingleOrDefault(x => "name".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            if (string.IsNullOrEmpty(name))
                return null;

            return name;
        }
    }
}
