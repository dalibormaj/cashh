using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Infrastructure.Repositories.Abstraction
{
    public interface ICashierRepository
    {
        Cashier SaveCashier(Cashier cashier);
        Cashier GetCashier(string cashierId);
        Cashier GetCashier(string cashier_id = "", string parentAgentId = "", string userName = "", int? companyId = null, int? venueId = null);
    }
}
