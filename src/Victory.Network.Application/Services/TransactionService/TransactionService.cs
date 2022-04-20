using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Victory.Network.Infrastructure.Errors;
using Victory.Network.Infrastructure.HttpClients.InternalApi;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Requests;

namespace Victory.Network.Application.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        public IInternalApiClient _internalApiClient;
        public TransactionService(ILogger<TransactionService> logger,
                                  IInternalApiClient internalApiClient)
        {
            _internalApiClient = internalApiClient;
        }

        //transfer funds between main wallets
        public async Task<long> TransferFunds(int fromUserId, int toUserId, decimal amount)
        {
            //TODO!!! check if transfer between fromUserId and toUserId is possible

            amount = Math.Abs(amount);
            var fromTransaction = await _internalApiClient.CreateTransaction(
                new CreateTransactionRequest()
                {
                    UserId = fromUserId.ToString(),
                    Amount = amount,
                    CapToBalance = false,
                    TxTypeId = (int)TransactionType.AGENT_WITHDRAWAL
                });

            if (!fromTransaction.SuccessCall) {
                var errors = fromTransaction.GetErrors();
                throw new VictoryNetworkException(errors);
            };

            var toTransaction = await _internalApiClient.CreateTransaction(
                new CreateTransactionRequest()
                {
                    UserId = toUserId.ToString(),
                    Amount = amount,
                    GroupIdentifier = fromTransaction.GroupIdentifier,
                    TxTypeId = (int)TransactionType.RETAIL_CASH_DEPOSIT
                });

            if (!toTransaction.SuccessCall)
            {
                var errors = toTransaction.GetErrors();
                throw new VictoryNetworkException(errors);
            };

            return toTransaction.TransactionId;
        }
    }
}
