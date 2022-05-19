using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
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

            if(!authHeaderValue.StartsWith("Bearer"))
                return AuthenticateResult.Fail($"Bearer token missing. Check if Authorization header starts with the Bearer key");

            var token = authHeaderValue.Substring("Bearer".Length).Trim();
            var validateResponse = await _guardianClient.ValidateTokenAsync(token);
            var isValid = validateResponse?.IsValidated ?? false;

            if (!isValid)
            {
                return AuthenticateResult.Fail($"Access denied");
            }

            var claims = GenerateClaims(validateResponse, token);
            var identity = new ClaimsIdentity(claims, nameof(GuardianAuthenticationHandler));
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
