using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Victory.Auth.HttpClients.Guardian;

namespace Victory.Auth
{
    public class VCashCashierAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILogger<VCashCashierAuthenticationHandler> _logger;
        private readonly IVCashAuthClient _vCashAuthClient;
        private readonly VCashAuthSettings _settings;

        public VCashCashierAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IVCashAuthClient vCashAuthClient, VCashAuthSettings settings)
            : base(options, logger, encoder, clock)
        {

            _logger = logger.CreateLogger<VCashCashierAuthenticationHandler>();
            _vCashAuthClient = vCashAuthClient;
            _settings = settings;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            const string BEARER = JwtBearerDefaults.AuthenticationScheme;
            var authHeaderValue = Request.Headers[HeaderNames.Authorization].ToString();

            if (!authHeaderValue.StartsWith(BEARER))
                return AuthenticateResult.Fail($"{BEARER} token missing. Check if Authorization header starts with the {BEARER} key");

            var token = authHeaderValue.Substring(BEARER.Length).Trim();

            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.CashierTokenKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var tokenHandler = new JwtSecurityTokenHandler();

                //validate cashier token
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = credentials.Key,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                }, out SecurityToken validatedToken);

                var deviceToken = claimsPrincipal.Claims.GetClaim("device_token");
                if (string.IsNullOrEmpty(deviceToken))
                    return AuthenticateResult.Fail($"Access denied. Device token missing");

                //check if device token is valid
                var authResponse = await _vCashAuthClient.ValidateDeviceTokenAsync(deviceToken);

                var isValid = !((authResponse?.Errors?.Any()) ?? false);//not valid if there is any error 
                if (!isValid)
                {
                    var errors = authResponse?.Errors?
                                              .ToDictionary(item => item.Code,
                                                            item => item.Description);

                    //Add failed reasons to the scope so it can be read if needed
                    Context.Items.Add(AuthContextConstants.AUTH_FAILED_REASONS, errors);

                    return AuthenticateResult.Fail($"Access denied", new AuthenticationProperties(errors));
                }

                var ticket = new AuthenticationTicket(claimsPrincipal, AuthScheme.VCASH_CASHIER);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                var errors = new Dictionary<string, string>();
                if (ex.Message.StartsWith("IDX10223"))
                    errors.Add("IDX10223", "Token has expired");
                else
                    errors.Add("Error", ex.Message);

                //Add failed reasons to the scope so it can be read if needed
                Context.Items.Add(AuthContextConstants.AUTH_FAILED_REASONS, errors);

                return AuthenticateResult.Fail("Access denied", new AuthenticationProperties(errors));
            }
        }
    }
}
