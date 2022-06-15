using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;
using Victory.VCash.Domain.Query;

namespace Victory.VCash.Infrastructure.Repositories.Abstraction
{
    public interface ICashierRepository
    {
        Cashier SaveCashier(Cashier cashier);
        Cashier GetCashier(Guid cashierId);
        List<Cashier> GetCashiers(Guid? cashierId = null, Guid? parentAgentId = null, string userName = "", int? companyId = null, int? venueId = null);
        List<CashierWithDetails> GetAllCashiersWithDetails(Guid agentId);
    }
}
