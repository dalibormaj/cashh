using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Victory.Auth.HttpClients.Guardian;
using Victory.Auth.HttpClients.Guardian.Dtos;

namespace Victory.Auth
{
    public class GuardianAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private IGuardianClient _guardianClient;
        private readonly ILogger<GuardianAuthenticationHandler> _logger;
        public GuardianAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
                                             ILoggerFactory logger, 
                                             UrlEncoder encoder, 
                                             ISystemClock clock, 
                                             IGuardianClient guardianClient)
            : base(options, logger, encoder, clock)
        {
            _guardianClient = guardianClient;
            _logger = logger.CreateLogger<GuardianAuthenticationHandler>();
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            const string BEARER = JwtBearerDefaults.AuthenticationScheme;
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
                return AuthenticateResult.Fail($"Authorization header missing");

            var authHeaderValue = Request.Headers[HeaderNames.Authorization].ToString();

            if(!authHeaderValue.StartsWith(BEARER))
                return AuthenticateResult.Fail($"{BEARER} token missing. Check if Authorization header starts with the {BEARER} key");

            var token = authHeaderValue.Substring(BEARER.Length).Trim();
            var validateResponse = await _guardianClient.ValidateTokenAsync(token);
            var isValid = validateResponse?.IsValidated ?? false;

            if (!isValid)
            {
                _logger.LogInformation("{handler} - Access denied for token: {token} - reason {reason}", nameof(GuardianAuthenticationHandler), token, validateResponse.ValidationError);
                return AuthenticateResult.Fail("Access denied");
            }

            var claims = Context.User.Claims?.ToList() ?? new List<Claim>(); //current claims. It should be empty in most cases
            var newClaims = GenerateClaims(validateResponse, token);
            claims.AddRange(newClaims);
            var identity = new ClaimsIdentity(claims, AuthScheme.GUARDIAN);

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), AuthScheme.GUARDIAN);

            return AuthenticateResult.Success(ticket);
        }

        private List<Claim> GenerateClaims(ValidateTokenResponse validateTokenResponse, string token)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(token)) claims.Add(new Claim("guardian_token", token));
            if (!string.IsNullOrEmpty(validateTokenResponse.UserId)) claims.Add(new Claim("user_id", validateTokenResponse.UserId));
            if (!string.IsNullOrEmpty(validateTokenResponse.UserName)) claims.Add(new Claim("user_name", validateTokenResponse.UserName));

            return claims;
        }
    }
}
