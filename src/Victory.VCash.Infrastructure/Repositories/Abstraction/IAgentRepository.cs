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
        Agent GetAgent(string agentId);
        Agent GetAgent(string agentId = null, int? userId = null, string userName = null);
        Company GetCompany(int companyId);
        Company GetCompany(string agentId);
        Venue GetVenue(int venueId);
        List<Venue> GetVenues(int companyId);
        Agent SaveAgent(Agent agent);
        Company SaveCompany(Company company);
        Venue SaveVenue(Venue venue);
    }
}
