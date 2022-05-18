using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Common;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.HttpClients.InternalApi;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Enums;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Requests;
using Victory.VCash.Infrastructure.Repositories;

namespace Victory.VCash.Application.Services.MoneyTransferService
{
    public class MoneyTransferProvider : IMoneyTransferProvider
    {
        private IInternalApiClient _internalApiClient;
        private IUnitOfWork _unitOfWork;

        public MoneyTransferProvider(IInternalApiClient internalApiClient,
                                     IUnitOfWork unitOfWork)
        {
            if (internalApiClient == null)
                throw new VCashException(ErrorCode.SYSTEM_ERROR, $"Instance of {nameof(internalApiClient)} missing");

            _internalApiClient = internalApiClient;
            _unitOfWork = unitOfWork;
        }

        public MoneyTransfer Create(int fromUserId, int toUserId, decimal amount)
        {
            var fromUser = _internalApiClient.GetUserDetailsAsync(new GetUserDetailsRequest() { TpsUserId = fromUserId.ToString() }).Result;
            var toUser = _internalApiClient.GetUserDetailsAsync(new GetUserDetailsRequest() { TpsUserId = toUserId.ToString() }).Result;

            if (amount == 0)
                throw new VCashException(ErrorCode.AMOUNT_MISSING_OR_ZERO);

            if (fromUser == null)
                throw new VCashException(ErrorCode.USER_DOES_NOT_EXIST);

            if (fromUser.UserStatus?.Code != PL_UserStatus.ACT.ToString())
                throw new VCashException(ErrorCode.USER_IS_NOT_ACTIVE);

            if (toUser == null)
                throw new VCashException(ErrorCode.USER_DOES_NOT_EXIST);

            if (toUser.UserStatus?.Code != PL_UserStatus.ACT.ToString())
                throw new VCashException(ErrorCode.USER_IS_NOT_ACTIVE);

            if (string.IsNullOrEmpty(fromUser.UserType?.Code) || string.IsNullOrEmpty(toUser.UserType?.Code))
                throw new VCashException(ErrorCode.BAD_REQUEST);


            var isDeposit = "PLYON".Equals(toUser.UserType.Code, StringComparison.OrdinalIgnoreCase);
            var needApprovalAmount = isDeposit ? 5000 : 500;
          
            var maxDepositAmount = 6000;
            var minDepositAmount = 100;

            var maxPayoutAmount = 2000;
            var minPayoutAmount = 100;

            if (isDeposit && (amount > maxDepositAmount || amount < minDepositAmount))
                throw new VCashException(ErrorCode.MONEY_TRANSFER_CANNOT_BE_PROCESSED, "due to invalid amount");

            if (!isDeposit && (amount > maxPayoutAmount || amount < minPayoutAmount))
                throw new VCashException(ErrorCode.MONEY_TRANSFER_CANNOT_BE_PROCESSED, "due to invalid amount");

            var moneyTransfer = new MoneyTransfer()
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Amount = amount,
                MoneyTransferStatusId = amount <= needApprovalAmount ? MoneyTransferStatus.APPROVED : MoneyTransferStatus.PENDING_APPROVAL
            };

            var fromTransactionTypeId = fromUser.UserType.Code?.ToUpper() switch
            {
                "AGENT" => (int)PL_TransactionType.AGENT_CASH_PAYOUT,
                "PLYON" => (int)PL_TransactionType.USER_CASH_PAYOUT,
                _ => throw new InvalidOperationException()
            };

            var toTransactionTypeId = toUser.UserType.Code?.ToUpper() switch
            {
                "AGENT" => (int)PL_TransactionType.AGENT_CASH_DEPOSIT,
                "PLYON" => (int)PL_TransactionType.USER_CASH_DEPOSIT,
                _ => throw new InvalidOperationException()
            };

            moneyTransfer.Transactions = new List<Transaction>() {
                new Transaction()
                {
                    ExternalTransactionTypeId = fromTransactionTypeId,
                    UserId = Convert.ToInt32(fromUser.UserId),
                    Amount = -1 * amount
                },
                new Transaction()
                {
                    ExternalTransactionTypeId = toTransactionTypeId,
                    UserId = Convert.ToInt32(toUser.UserId),
                    Amount = amount
                }
            };

            moneyTransfer = _unitOfWork.GetRepository<MoneyTransferRepository>().SaveMoneyTransfer(moneyTransfer);
            return moneyTransfer;
        }

        public async Task<MoneyTransfer> ProcessAsync(long moneyTransferId, MoneyTransferStatus? newStatus = null)
        {
            var moneyTransfer = _unitOfWork.GetRepository<MoneyTransferRepository>().GetMoneyTransfer(moneyTransferId);
            var isRetry = newStatus != null && moneyTransfer.MoneyTransferStatusId == newStatus;

            if (isRetry)
                return moneyTransfer;

            newStatus = newStatus ?? moneyTransfer.MoneyTransferStatusId;
            if (!CanProcess(moneyTransfer.MoneyTransferStatusId, (MoneyTransferStatus)newStatus))
                throw new VCashException(ErrorCode.MONEY_TRANSFER_CANNOT_BE_PROCESSED);

            Func<MoneyTransfer, Transaction, bool> canProcessTransaction = (moneyTransfer, transaction) =>
            {
                //in case of pending approval do not settle deposit transactions
                //but payout only so the funds can be reserved.
                return (moneyTransfer.MoneyTransferStatusId == MoneyTransferStatus.PENDING_APPROVAL &&
                        new[] { (int?)PL_TransactionType.AGENT_CASH_PAYOUT,
                                (int?)PL_TransactionType.USER_CASH_PAYOUT }.Contains(transaction.ExternalTransactionTypeId)) ||
                        moneyTransfer.MoneyTransferStatusId != MoneyTransferStatus.PENDING_APPROVAL;
            };

            if (moneyTransfer.Transactions == null ||
               !moneyTransfer.Transactions.Any() ||
                moneyTransfer.Transactions.Where(x => x.ExternalTransactionTypeId == null)
                                          .Any())
                throw new VCashException(ErrorCode.INVALID_MONEY_TRANSFER);

            try
            {
                long? groupIdentifier = null;

                foreach (var transaction in moneyTransfer.Transactions)
                {
                    if (canProcessTransaction(moneyTransfer, transaction))
                    {
                        //settle transaction by calling platform internal api
                        if (transaction.ExternalTransactionId == null)
                        {
                            var result = await _internalApiClient.CreateTransactionAsync(new CreateTransactionRequest()
                            {
                                UserId = transaction.UserId.ToString(),
                                Amount = Math.Abs(transaction.Amount),
                                CapToBalance = false,
                                GroupIdentifier = groupIdentifier,
                                TxTypeId = transaction.ExternalTransactionTypeId.Value
                            });
                            result.ThrowIfNotSuccess();

                            //all transactions should have the same
                            //group identifier so it can be linked together
                            groupIdentifier = result.GroupIdentifier;
                            transaction.ExternalTransactionId = result.TransactionId;
                        }
                        else
                        {
                            var statusMapper = new Dictionary<MoneyTransferStatus, PL_TransactionStatus>() {
                                { MoneyTransferStatus.PENDING_APPROVAL, PL_TransactionStatus.COMPLETE },
                                { MoneyTransferStatus.APPROVED, PL_TransactionStatus.COMPLETE },
                                { MoneyTransferStatus.REJECTED, PL_TransactionStatus.VOID },
                                { MoneyTransferStatus.REFUNDED, PL_TransactionStatus.VOID },
                                { MoneyTransferStatus.COMPLETED, PL_TransactionStatus.COMPLETE }
                            };

                            var result = await _internalApiClient.UpdateTransactionAsync(new UpdateTransactionRequest()
                            {
                                TransactionIdentifier = transaction.ExternalTransactionId.Value,
                                TransactionTypeId = transaction.ExternalTransactionTypeId.Value,
                                IpAddress = NetworkHelper.GetLocalIp(),
                                NewTransactionStatusId = (int)statusMapper[(MoneyTransferStatus)newStatus],
                            });
                            result.ThrowIfNotSuccess();
                        }
                    }

                    //link returned platform data to the transaction
                    transaction.ExternalGroupIdentifier = groupIdentifier;
                    transaction.MoneyTransferId = moneyTransfer.MoneyTransferId;

                    //save the result
                    _unitOfWork.GetRepository<MoneyTransferRepository>().SaveTransaction(transaction);
                }

                moneyTransfer.MoneyTransferStatusId = (MoneyTransferStatus)newStatus;
                moneyTransfer = _unitOfWork.GetRepository<MoneyTransferRepository>().SaveMoneyTransfer(moneyTransfer);

                return moneyTransfer;
            }
            catch (Exception ex)
            {
                //save error 
                moneyTransfer.MoneyTransferStatusId = MoneyTransferStatus.ERROR;
                moneyTransfer.Error = ex.Message;
                moneyTransfer = _unitOfWork.GetRepository<MoneyTransferRepository>().SaveMoneyTransfer(moneyTransfer);

                //no need to refund in case of INSUFFICIENT_FUNDS
                if (ex is VCashException vex)
                    if (vex.Errors?.Exists(x => x.ErrorCode == ErrorCode.INSUFFICIENT_FUNDS) ?? false)
                        throw;

                //try to refund. Do not execute if the refund fails,
                //otherwise we can end up in infinite loop
                if (newStatus != MoneyTransferStatus.REFUNDED)
                    await ProcessAsync(moneyTransferId, MoneyTransferStatus.REFUNDED);
                throw;
            }
        }

        public static bool CanProcess(MoneyTransferStatus? oldStatus, MoneyTransferStatus newStatus)
        {
            var rules = new List<KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>>()
            {
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(null, MoneyTransferStatus.PENDING_APPROVAL),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(null, MoneyTransferStatus.APPROVED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.APPROVED, MoneyTransferStatus.APPROVED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.APPROVED, MoneyTransferStatus.COMPLETED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.APPROVED, MoneyTransferStatus.REJECTED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.APPROVED, MoneyTransferStatus.REFUNDED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.APPROVED, MoneyTransferStatus.PENDING_APPROVAL),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.APPROVED, MoneyTransferStatus.ERROR),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.PENDING_APPROVAL, MoneyTransferStatus.PENDING_APPROVAL),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.PENDING_APPROVAL, MoneyTransferStatus.REJECTED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.PENDING_APPROVAL, MoneyTransferStatus.ERROR),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.PENDING_APPROVAL, MoneyTransferStatus.APPROVED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.COMPLETED, MoneyTransferStatus.REFUNDED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.COMPLETED, MoneyTransferStatus.COMPLETED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.REFUNDED, MoneyTransferStatus.REFUNDED),
                new KeyValuePair<MoneyTransferStatus?, MoneyTransferStatus>(MoneyTransferStatus.REJECTED, MoneyTransferStatus.REJECTED)
            };

            return rules.Exists(x => x.Key == oldStatus && x.Value == newStatus);
        }
    }
}
