using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.Repositories;

namespace Victory.VCash.Application.Services.MoneyTransferService
{
    public class MoneyTransferService : IMoneyTransferService
    {
        ILogger<MoneyTransferService>  _logger;
        IMoneyTransferProvider _provider;
        IUnitOfWork _unitOfWork;    

        public MoneyTransferService(ILogger<MoneyTransferService> logger,
                                    IMoneyTransferProvider moneyTransferProvider,
                                    IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _provider = moneyTransferProvider;
            _unitOfWork = unitOfWork;
        }


        //transfer funds between users
        public async Task<MoneyTransfer> CreateAsync(int fromUserId, int toUserId, decimal amount, string cashierId = "")
        {
            var moneyTransfer = _provider.Create(fromUserId, toUserId, amount, cashierId);

            if (moneyTransfer.MoneyTransferStatusId == MoneyTransferStatus.PENDING_APPROVAL) 
                return await _provider.ProcessAsync(moneyTransfer.MoneyTransferId.Value);

            return await _provider.ProcessAsync(moneyTransfer.MoneyTransferId.Value, MoneyTransferStatus.COMPLETED);
        }
        public async Task<MoneyTransfer> RefundAsync(long moneyTransferId)
        {
            return await _provider.ProcessAsync(moneyTransferId, MoneyTransferStatus.REFUNDED);
        }

        public async Task<MoneyTransfer> ApproveAsync(long moneyTransferId)
        {
            return await _provider.ProcessAsync(moneyTransferId, MoneyTransferStatus.APPROVED);
        }
        public async Task<MoneyTransfer> RejectAsync(long moneyTransferId)
        {
            return await _provider.ProcessAsync(moneyTransferId, MoneyTransferStatus.REJECTED);
        }

        /// <summary>
        /// It can be used to fix broken transfers that are stuck in some status.
        /// For instance if transfer is APPROVED but some transactions didn't complete
        /// on the platform side, it can fix the problem by creating missing transaction
        /// Note: Money transfer status won't be changed
        /// </summary>
        /// <param name="moneyTransferId"></param>
        /// <returns></returns>

        public async Task<MoneyTransfer> RefreshAsync(long moneyTransferId)
        {
            return await _provider.ProcessAsync(moneyTransferId);
        }

        public MoneyTransfer GetMoneyTransfer(long moneyTransferId)
        {
            var moneyTransfer = _unitOfWork.GetRepository<MoneyTransferRepository>().GetMoneyTransfer(moneyTransferId);
            return moneyTransfer;
        }

        public List<MoneyTransfer> GetMoneyTransfers(long? moneyTransferId = null, int? fromUserId = null, int? toUserId = null, decimal? amountFrom = null, decimal? amountTo = null, DateTime? dateFrom = null, DateTime? dateTo = null, MoneyTransferStatus? status = null)
        {
            var moneyTransfers = _unitOfWork.GetRepository<MoneyTransferRepository>().GetMoneyTransfers(moneyTransferId , fromUserId, toUserId, amountFrom, amountTo, dateFrom, dateTo, status);
            return moneyTransfers;
        }
    }
}
