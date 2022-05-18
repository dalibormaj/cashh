using System;
using System.Collections.Generic;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Infrastructure.Repositories.Abstraction
{
    public interface IMoneyTransferRepository
    {
        MoneyTransfer SaveMoneyTransfer(MoneyTransfer moneyTransfer);
        Transaction SaveTransaction(Transaction transaction);
        IEnumerable<Transaction> SaveTransactions(List<Transaction> transactions);
        MoneyTransfer GetMoneyTransfer(long moneyTransferId);
        List<MoneyTransfer> GetMoneyTransfers(long? moneyTransferId = null, int? fromUserId = null, int? toUserId = null, decimal? amountFrom = null, decimal? amountTo = null, DateTime? dateFrom = null, DateTime? dateTo = null, MoneyTransferStatus? status = null);
        List<Transaction> GetTransactions(long moneyTransferId);
    }
}
