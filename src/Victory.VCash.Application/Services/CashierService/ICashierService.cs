using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;
using Victory.VCash.Domain.Query;

namespace Victory.VCash.Application.Services.CashierService
{
    public interface ICashierService
    {
        Cashier GetCashierByUserName(string userName);
        List<CashierWithDetails> GetCashiers(Guid agentId);
        Cashier Register(Guid parentAgentId, int venueId, string userName, string firstName, string lastName, string pin = null);
        Cashier Deregister(Guid cashierId);
    }
}
