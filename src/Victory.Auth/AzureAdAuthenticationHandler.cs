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

        public AzureAdAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, OpenIdConnectConfiguration config)
            : base(options, logger, encoder, clock)
        {
            _config = config;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeaderValue = Request.Headers[HeaderNames.Authorization].ToString();
            if (!authHeaderValue.StartsWith(AuthSchema.AZURE_AD))
                return Task.FromResult(AuthenticateResult.Fail($"{AuthSchema.AZURE_AD} token missing. Check if Authorization header starts with the {AuthSchema.AZURE_AD} key"));

            try
            {
                var token = authHeaderValue.Substring(AuthSchema.AZURE_AD.Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKeys = _config.SigningKeys,
                    ValidateAudience = false,
                    ValidIssuers = new[] { _config.Issuer }
                }, out SecurityToken validatedToken);
            
                var ticket = new AuthenticationTicket(claimsPrincipal, AuthSchema.AZURE_AD);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                var reason = string.Empty;
                if (ex.Message.StartsWith("IDX10223"))
                    reason = "Token has expired.";
                return Task.FromResult(AuthenticateResult.Fail($"Access denied. {reason}".Trim()));
            }
        }
    }
}
