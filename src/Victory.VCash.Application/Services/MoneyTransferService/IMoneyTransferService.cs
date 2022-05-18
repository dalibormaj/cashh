using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Application.Services.MoneyTransferService
{
    public interface IMoneyTransferService
    {
        Task<MoneyTransfer> CreateAsync(int fromUserId, int toUserId, decimal amount);
        Task<MoneyTransfer> RefundAsync(long moneyTransferId);
        Task<MoneyTransfer> ApproveAsync(long moneyTransferId);
        Task<MoneyTransfer> RejectAsync(long moneyTransferId);
        Task<MoneyTransfer> RefreshAsync(long moneyTransferId);

        MoneyTransfer GetMoneyTransfer(long moneyTransferId);
        List<MoneyTransfer> GetMoneyTransfers(long? moneyTransferId = null, int? fromUserId = null, int? toUserId = null, decimal? amountFrom = null, decimal? amountTo = null, DateTime? dateFrom = null, DateTime? dateTo = null, MoneyTransferStatus? status = null);
    }
}
