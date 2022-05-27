using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Victory.DataAccess;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.Repositories;

namespace Victory.VCash.Application.Services.CashierService
{
    public class CashierService : ICashierService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CashierService(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }

        public Cashier GetCashierByUserName(string userName)
        {
            var cashier = _unitOfWork.GetRepository<CashierRepository>().GetCashier(userName: userName);
            if (cashier == null)
                throw new VCashException(ErrorCode.CASHIER_DOES_NOT_EXIST);

            return cashier;
        }

        public Cashier Register(string parentAgentId, int shopId, string userName, string name, string lastName)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(parentAgentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_DOES_NOT_EXIST);

            //TODO!!! ovde proveriti da li proslednjei shop pripada agentu

            var cashier = new Cashier()
            {
                ParentAgentId = parentAgentId,
                ShopId = shopId,
                UserName = userName,
                Name = name,
                LastName = lastName,
                Pin = new Random().Next(1000, 9999).ToString()
            };
            cashier = _unitOfWork.GetRepository<CashierRepository>().SaveCashier(cashier);
            return cashier;
        }

        public Cashier Deregister(string cashierId)
        {
            throw new NotImplementedException();
        }

        public string CreateAccessToken(string parentAgentId, string userName, string pin)
        {
            var parentAgent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(parentAgentId);
            if (parentAgent == null)
                throw new VCashException(ErrorCode.AGENT_DOES_NOT_EXIST);

            var cashier = _unitOfWork.GetRepository<CashierRepository>().GetCashier(parentAgentId: parentAgentId, userName: userName);
            if (cashier == null)
                throw new VCashException(ErrorCode.CASHIER_DOES_NOT_EXIST);

            var isPinValid = cashier.Pin == pin;
            if (!isPinValid)
                throw new UnauthorizedAccessException();

            //create token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("cashier_id", cashier.CashierId),
                    new Claim("cashier_parent_agent_id", cashier.ParentAgentId),
                    new Claim("cashier_parent_agent_user_id", parentAgent.UserId.ToString()),
                    new Claim("cashier_user_name", cashier.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(2),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public AccessTokenValidationResult ValidateAccessToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(accessToken)?.Claims;

            string exp = claims?.ToList().SingleOrDefault(x => "exp".Equals(x.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            if (!string.IsNullOrEmpty(exp))
            {
                var expirationDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(exp)).DateTime;
                if (expirationDate <= DateTime.UtcNow)
                    return new AccessTokenValidationResult()
                    {
                        Claims = claims.ToList(),
                        IsValid = false,
                        ErrorMessage = "Token has expired"
                    };
            };

            return new AccessTokenValidationResult()
            {
                Claims = claims.ToList(),
                IsValid = true
            };
        }
    }
}
