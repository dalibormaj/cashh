using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Victory.DataAccess;
using Victory.VCash.Domain.Models;
using Victory.VCash.Domain.Query;
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
            var cashiers = _unitOfWork.GetRepository<CashierRepository>().GetCashiers(userName: userName);
            if(cashiers == null || (cashiers?.Count()?? 0) > 1)
                throw new VCashException(ErrorCode.CASHIER_CANNOT_BE_FOUND);

            return cashiers.FirstOrDefault();
        }

        public Cashier Register(Guid parentAgentId, int venueId, string userName, string firstName, string lastName, string pin = null)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(parentAgentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            var venue = _unitOfWork.GetRepository<AgentRepository>().GetVenue(venueId);
            if (venue == null)
                throw new VCashException(ErrorCode.VENUE_CANNOT_BE_FOUND);

            if (venue.CompanyId != agent.CompanyId)
                throw new VCashException(ErrorCode.VENUE_DOES_NOT_BELONG_TO_THE_AGENT);

            var isExist = _unitOfWork.GetRepository<CashierRepository>().GetCashiers(parentAgentId: agent.AgentId.Value, userName: userName)?.Any() ?? false;
            if (isExist)
                throw new VCashException(ErrorCode.CASHIER_ALREADY_REGISTERED);

            //generate random pin if it's not selected
            pin = string.IsNullOrEmpty(pin) ? new Random().Next(1000, 9999).ToString() : pin;

            var cashier = new Cashier()
            {
                ParentAgentId = parentAgentId,
                VenueId = venueId,
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Pin = pin
            };
            cashier = _unitOfWork.GetRepository<CashierRepository>().SaveCashier(cashier);
            return cashier;
        }

        public Cashier Deregister(Guid cashierId)
        {
            throw new NotImplementedException();
        }

        public List<CashierWithDetails> GetCashiers(Guid agentId)
        {
            return _unitOfWork.GetRepository<CashierRepository>().GetAllCashiersWithDetails(agentId);
        }
    }
}
