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
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Victory.Auth.HttpClients.Guardian;
using Victory.Auth.HttpClients.VCashAuth.Dtos;

namespace Victory.Auth
{
    public class VCashDeviceAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILogger<VCashDeviceAuthenticationHandler> _logger;
        private readonly IVCashAuthClient _vCashAuthClient;

        public VCashDeviceAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IVCashAuthClient vCashAuthClient)
            : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger<VCashDeviceAuthenticationHandler>();
            _vCashAuthClient = vCashAuthClient;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            const string BEARER = JwtBearerDefaults.AuthenticationScheme;
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
                return AuthenticateResult.Fail($"Authorization header missing");

            var authHeaderValue = Request.Headers[HeaderNames.Authorization].ToString();

            if (!authHeaderValue.StartsWith(BEARER))
                return AuthenticateResult.Fail($"{BEARER} token missing. Check if Authorization header starts with the {JwtBearerDefaults.AuthenticationScheme} key");

            var token = authHeaderValue.Substring(BEARER.Length).Trim();
            var authResponse = await _vCashAuthClient.ValidateDeviceTokenAsync(token);

            var isValid = !((authResponse?.Errors?.Any()) ?? false);//not valid if there is any error 
            if (!isValid)
            {
                var reason = authResponse.Errors[0].Description;//get first error received
                _logger.LogInformation("{handler} - Access denied for token: {token} - reason {reason}", nameof(VCashDeviceAuthenticationHandler), token, reason);
                return AuthenticateResult.Fail($"Access denied. {reason}");
            }
        
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(token)?.Claims?.ToList();
            var identity = new ClaimsIdentity(claims, AuthScheme.VCASH_DEVICE);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), AuthScheme.VCASH_DEVICE);

            return AuthenticateResult.Success(ticket);
        }
    }
}
