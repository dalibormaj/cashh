using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Application.Services.CashierService
{
    public interface ICashierService
    {
        Cashier GetCashierByUserName(string userName);
        Cashier Register(string parentAgentId, int shopId, string userName, string name, string lastName);
        string CreateAccessToken(string parentAgentId, string userName, string pin);
        AccessTokenValidationResult ValidateAccessToken(string accessToken);
    }
}
