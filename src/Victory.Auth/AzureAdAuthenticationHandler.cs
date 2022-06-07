using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Victory.Auth
{
    public class AzureAdAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private OpenIdConnectConfiguration _config;
        private readonly ILogger<AzureAdAuthenticationHandler> _logger;

        public AzureAdAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, OpenIdConnectConfiguration config)
            : base(options, logger, encoder, clock)
        {
            _config = config;
            _logger = logger.CreateLogger<AzureAdAuthenticationHandler>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            const string BEARER = JwtBearerDefaults.AuthenticationScheme;
            var authHeaderValue = Request.Headers[HeaderNames.Authorization].ToString();
            
            if (!authHeaderValue.StartsWith(BEARER))
                return Task.FromResult(AuthenticateResult.Fail($"{BEARER} token missing. Check if Authorization header starts with the {BEARER} key"));

            var token = authHeaderValue.Substring(BEARER.Length).Trim();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKeys = _config.SigningKeys,
                    ValidateAudience = false,
                    ValidIssuers = new[] { _config.Issuer }
                }, out SecurityToken validatedToken);
            
                var ticket = new AuthenticationTicket(claimsPrincipal, AuthScheme.AZURE_AD);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                var reason = ex.Message;
                if (ex.Message.StartsWith("IDX10223"))
                    reason = "Token has expired.";

                _logger.LogInformation("{handler} - Access denied for token: {token} - reason {reason}", nameof(AzureAdAuthenticationHandler), token, reason);
                return Task.FromResult(AuthenticateResult.Fail($"Access denied. {reason}".Trim()));
            }
        }
    }
}
