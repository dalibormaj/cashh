using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.VCash.Application.Services.MoneyTransferService
{
    internal static class ErrorMapperExtensions
    {
        public static List<Error> GetErrors(this CreateTransactionResponse response)
        {
            if (string.IsNullOrEmpty(response.ErrorMessage) && string.IsNullOrEmpty(response.ErrorCode))
                return null;

            var errors = new List<Error>();
            var errorMessage = string.IsNullOrEmpty(response.ErrorMessage) ? null : $"{response.ErrorMessage}";
            errors.Add(response.ErrorCode switch
            {
                "UserDoesNotHaveEnoughBalance" => new Error(ErrorCode.INSUFFICIENT_FUNDS),
                "UserAccountWasNotFound" => new Error(ErrorCode.USER_ACCOUNT_NOT_EXIST),
                _ => new Error(ErrorCode.BAD_REQUEST, errorMessage)
            });
            return errors;
        }

        public static List<Error> GetErrors(this UpdateTransactionResponse response)
        {
            if (string.IsNullOrEmpty(response.ErrorMessage) && string.IsNullOrEmpty(response.ErrorCode))
                return null;

            var errors = new List<Error>();
            var errorMessage = string.IsNullOrEmpty(response.ErrorMessage) ? null : $"{response.ErrorMessage}";
            errors.Add(response.ErrorCode switch
            {
                "TRANSACTION_STATUS_INVALID_ERROR" => new Error(ErrorCode.SYSTEM_ERROR, "Invalid platform transaction status configuration. Transaction status cannot be changed!"),
                _ => new Error(ErrorCode.BAD_REQUEST, errorMessage)
            });
            return errors;
        }

        public static void ThrowIfNotSuccess(this CreateTransactionResponse response)
        {
            if (!response.SuccessCall)
            {
                var errors = response.GetErrors();
                throw new VCashException(errors);
            };
        }

        public static void ThrowIfNotSuccess(this UpdateTransactionResponse response)
        {
            if (!response.SuccessCall)
            {
                var errors = response.GetErrors();
                throw new VCashException(errors);
            };
        }
    }
}
