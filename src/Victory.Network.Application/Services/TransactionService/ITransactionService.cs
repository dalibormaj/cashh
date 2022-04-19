using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.Network.Application.Services.TransactionService
{
    public interface ITransactionService
    {
        Task<long> TransferFunds(int fromUserId, int toUserId, decimal amount);
    }
}
