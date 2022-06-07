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

        public Cashier Register(string parentAgentId, int venueId, string userName, string name, string lastName)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(parentAgentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_DOES_NOT_EXIST);

            //TODO!!! ovde proveriti da li proslednjei shop pripada agentu

            var cashier = new Cashier()
            {
                ParentAgentId = parentAgentId,
                VenueId = venueId,
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
    }
}
