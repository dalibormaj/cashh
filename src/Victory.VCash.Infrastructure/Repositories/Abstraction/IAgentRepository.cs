using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Infrastructure.Repositories.Abstraction
{
    public interface IAgentRepository
    {
        Agent GetAgent(Guid agentId);
        List<Agent> GetAgents(Guid? agentId = null, int? userId = null, string userName = null, string email = null, int? companyId = null);
        Company GetCompany(int companyId);
        List<Company> GetCompanies(int? companyId = null, string taxNumber = null);
        Venue GetVenue(int venueId);
        List<Venue> GetVenues(int? companyId = null);
        BankAccount GetBankAccount(int bankAccountId);
        List<BankAccount> GetBankAccounts(int? bankAccountId = null, int? companyId = null);
        
        Agent SaveAgent(Agent agent);
        Company SaveCompany(Company company);
        Venue SaveVenue(Venue venue);
        BankAccount SaveBankAccount(BankAccount bankAccount);
    }
}
