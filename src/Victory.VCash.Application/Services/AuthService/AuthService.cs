using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Victory.DataAccess;
using Victory.VCash.Application.Services.AuthService.Results;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.Repositories;
using Victory.VCash.Infrastructure.Repositories.Settings;

namespace Victory.VCash.Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthServiceSettings _settings;

        public AuthService(ILogger<AuthService> logger,
                           AuthServiceSettings settings, 
                           IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _settings = settings;
            _unitOfWork = unitOfWork;
        }

        public Device RegisterDevice(Guid agentId, string deviceName)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            if(agent.AgentStatusId != AgentStatus.ACTIVE)
                throw new VCashException(ErrorCode.AGENT_IS_NOT_ACTIVE);

            //if all devices registered for selected agentId
            var devices = _unitOfWork.GetRepository<AuthRepository>().GetDevices(agentId);

            //if there are more than 1 device with the same name throw an error
            if (!string.IsNullOrEmpty(deviceName) && devices.Where(x => string.Equals(x.Name, deviceName, StringComparison.OrdinalIgnoreCase))?.Count() > 1)
                throw new VCashException(ErrorCode.BAD_REQUEST);

            //use existing device if device with device name exists
            //otherwise create a new one.
            var device = devices.Where(x => !string.IsNullOrEmpty(deviceName) &&
                                            string.Equals(x.Name, deviceName, StringComparison.OrdinalIgnoreCase))?
                                .FirstOrDefault() ?? new Device()
                                {
                                    AgentId = agentId,
                                    Name = deviceName
                                };

            //generate new device code, try again if it already exists
            //so we can get unique value per agent
            Func<List<Device>, string> generateDeviceCode = null;
            generateDeviceCode = (devices) =>
            {
                var newDeviceCode = $"VCASH{new Random().Next(10000, 99999)}";
                if (devices?.Exists(x => x.DeviceCode.Equals(newDeviceCode, StringComparison.OrdinalIgnoreCase)) ?? false)
                    return generateDeviceCode(devices);
                return newDeviceCode;
            };

            device.Enabled = false;
            device.DeviceCodeIssuedAt = DateTime.UtcNow;
            device.DeviceCodeExpiresInMin = _settings.DeviceToken.DeviceCodeExpiresInMin;
            device.DeviceCode = generateDeviceCode(devices);
            device.Token = null;
            device.TokenIssuedAt = null;
            device.TokenExpiresAt = null;

            device = _unitOfWork.GetRepository<AuthRepository>().SaveDevice(device);

            return device;
        }

        public CreateDeviceTokenResult CreateDeviceToken(Guid agentId, string deviceCode)
        {
            var devices = _unitOfWork.GetRepository<AuthRepository>().GetDevices(agentId, deviceCode: deviceCode);
            if (!(devices?.Any()?? false))
                throw new VCashException(ErrorCode.INVALID_DEVICE_CODE);

            if (devices?.Count() > 1)
                throw new VCashException(ErrorCode.INVALID_DEVICE_CODE);

            var device = devices?.FirstOrDefault();
            if (device.DeviceCodeIssuedAt != null && device.DeviceCodeIssuedAt.Value.AddMinutes(device.DeviceCodeExpiresInMin.Value) > DateTime.UtcNow)
                throw new VCashException(ErrorCode.DEVICE_CODE_HAS_EXPIRED);

            //create a token if device is approved
            if (device?.Enabled ?? false)
            {
                var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId);
                if (agent == null)
                    throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

                var tokenIssuedAt = DateTime.UtcNow;
                var tokenExpiresAt = DateTime.UtcNow.AddYears(_settings.DeviceToken.ExpiresInYears);

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.DeviceToken.SecurityKey));
                securityKey.KeyId = _settings.DeviceToken.KeyId;
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = new[]
                    {
                        new Claim("device_id", device.DeviceId.ToString()),
                        new Claim("agent_id", device.AgentId.ToString()),
                        new Claim("agent_user_id", agent.UserId.ToString())
                    };

                if (!string.IsNullOrEmpty(device.Name))
                    claims.Append(new Claim("device_name", device.Name));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    IssuedAt = tokenIssuedAt,
                    Expires = tokenExpiresAt,
                    SigningCredentials = credentials
                };
                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                device.Token = tokenString;
                device.TokenIssuedAt = tokenIssuedAt;
                device.TokenExpiresAt = tokenExpiresAt;

                _unitOfWork.GetRepository<AuthRepository>().SaveDevice(device);

                return new CreateDeviceTokenResult()
                {
                    DeviceToken = tokenString,
                    ExpiresAt = tokenExpiresAt
                };
            }

            throw new VCashException(ErrorCode.DEVICE_NOT_AUTHORIZED);
        }

        public ValidateDeviceTokenResult ValidateDeviceToken(string token)
        {
            var errors = new List<Error>();
            var claims = new List<Claim>();
            int deviceId = default;
            string deviceName = string.Empty;
            Guid? agentId = null;
            DateTime? issuedAt = null;
            DateTime? expiresAt = null;

            Func<IEnumerable<Claim>, string, string> getClaim = (claims, name) => {
                return claims.SingleOrDefault(x => x.Type.Equals(name, StringComparison.OrdinalIgnoreCase))?.Value;
            };

            try
            {
                token = token?.Trim();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.DeviceToken.SecurityKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = credentials.Key,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                }, out SecurityToken validatedToken);

                claims = claimsPrincipal.Claims.ToList();
                agentId = new Guid(getClaim(claims, "agent_id"));
                int.TryParse(getClaim(claims, "device_id"), out deviceId);
                deviceName = getClaim(claims, "device_name");

                if (deviceId == default)
                    throw new VCashException(ErrorCode.INVALID_DEVICE_TOKEN);

                var device = _unitOfWork.GetRepository<AuthRepository>().GetDevice(deviceId);
                if (device == null)
                    throw new VCashException(ErrorCode.DEVICE_CANNOT_BE_FOUND);

                if (device.AgentId != agentId)
                    throw new VCashException(ErrorCode.INVALID_DEVICE_TOKEN);

                if (!(device.Enabled ?? false))
                    throw new VCashException(ErrorCode.DEVICE_NOT_AUTHORIZED);

                string iat = getClaim(claims, "iat");
                if (!string.IsNullOrEmpty(iat))
                    issuedAt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(iat)).DateTime;

                string exp = getClaim(claims, "exp");
                if (!string.IsNullOrEmpty(exp))
                    expiresAt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(exp)).DateTime;

                var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId.Value);
                var isAgentActive = agent?.AgentStatusId == AgentStatus.ACTIVE;

                //fix values if someone manually changed in DB 
                if (device.TokenIssuedAt != issuedAt || device.TokenExpiresAt != expiresAt || !isAgentActive)
                {
                    device.TokenIssuedAt = issuedAt;
                    device.TokenExpiresAt = expiresAt;

                    //disable device if agent is not active
                    if (!isAgentActive)
                        device.Enabled = false;

                    device = _unitOfWork.GetRepository<AuthRepository>().SaveDevice(device);
                }

                if (!(device.Enabled ?? false))
                    throw new VCashException(ErrorCode.DEVICE_NOT_AUTHORIZED);
            }
            catch (VCashException ex)
            {
                errors = ex.Errors;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("IDX10223"))
                    errors.Add(new Error(ErrorCode.INVALID_DEVICE_TOKEN));
                else
                    errors.Add(new Error(ErrorCode.INVALID_DEVICE_TOKEN, ex.Message));
            }

            var isValid = !errors.Any();
            return new ValidateDeviceTokenResult()
            {
                IsValid = isValid,
                DeviceId = deviceId,
                DeviceName = deviceName,
                AgentId = agentId,
                IssuedAt = issuedAt,
                ExpiresAt = expiresAt,
                Claims = claims,
                Errors = errors.Distinct().ToList()
            };
        }

        public CreateCashierTokenResult CreateCashierToken(string deviceToken, string userName, string pin)
        {
            //clean device token
            var BEARER = "Bearer";
            deviceToken = deviceToken?.Substring(BEARER.Length).Trim();

            //read device claims
            var tokenHandler = new JwtSecurityTokenHandler();
            var deviceClaims = tokenHandler.ReadJwtToken(deviceToken)?.Claims?.ToList();
            var agentId = deviceClaims.SingleOrDefault(x => "agent_id".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;

            //validate pin
            var cashiers = _unitOfWork.GetRepository<CashierRepository>().GetCashiers(parentAgentId: new Guid(agentId), userName: userName);
            if (cashiers == null || (cashiers?.Count() ?? 0) > 1)
                throw new VCashException(ErrorCode.CASHIER_CANNOT_BE_FOUND);

            var cashier = cashiers.First();
            var isPinValid = cashier.Pin == pin;
            if (!isPinValid)
                throw new VCashException(ErrorCode.INVALID_CASHIER_CREDENTIALS);

            //create token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.CashierToken.SecurityKey));
            securityKey.KeyId = _settings.CashierToken.KeyId;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim("device_token", deviceToken),
                new Claim("cashier_id", cashier.CashierId),
                new Claim("cashier_user_name", cashier.UserName)
            };

            claims.AddRange(deviceClaims); //merge device and cashier claims

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return new CreateCashierTokenResult()
            {
                AccessToken = tokenHandler.WriteToken(token)
            };
        }
    }
}
