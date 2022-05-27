using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Victory.Auth.HttpClients.Guardian;
using Victory.Auth.HttpClients.Guardian.Models;

namespace Victory.Auth
{
    public class GuardianAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private IGuardianClient _guardianClient;
        public GuardianAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
                                             ILoggerFactory logger, 
                                             UrlEncoder encoder, 
                                             ISystemClock clock, 
                                             IGuardianClient guardianClient)
            : base(options, logger, encoder, clock)
        {
            _guardianClient = guardianClient;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey(HeaderNames.Authorization))
                return AuthenticateResult.Fail($"Authorization header missing");

            var authHeaderValue = Request.Headers[HeaderNames.Authorization].ToString();

            if(!authHeaderValue.StartsWith(AuthSchema.BEARER))
                return AuthenticateResult.Fail($"{AuthSchema.BEARER} token missing. Check if Authorization header starts with the {AuthSchema.BEARER} key");

            var token = authHeaderValue.Substring(AuthSchema.BEARER.Length).Trim();
            var validateResponse = await _guardianClient.ValidateTokenAsync(token);
            var isValid = validateResponse?.IsValidated ?? false;

            if (!isValid)
            {
                return AuthenticateResult.Fail($"Access denied");
            }

            var claims = Context.User.Claims?.ToList() ?? new List<Claim>(); //current claims. It should be empty in most cases
            var newClaims = GenerateClaims(validateResponse, token);
            claims.AddRange(newClaims);
            var identity = new ClaimsIdentity(claims, AuthSchema.BEARER);

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), AuthSchema.BEARER);

            return AuthenticateResult.Success(ticket);
        }

        private List<Claim> GenerateClaims(ValidateTokenResponse validateTokenResponse, string token)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(token)) claims.Add(new Claim("Token", token));
            if (!string.IsNullOrEmpty(validateTokenResponse.UserId)) claims.Add(new Claim("UserId", validateTokenResponse.UserId));
            if (!string.IsNullOrEmpty(validateTokenResponse.UserName)) claims.Add(new Claim("UserName", validateTokenResponse.UserName));

            return claims;
        }
    }
}
