using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.Network.Infrastructure.Errors;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.Network.Application.Services.TransactionService
{
    internal static class ErrorMapperExtensions
    {
        public static List<ErrorCode> GetErrors(this CreateTransactionResponse response)
        {
            var errors = new List<ErrorCode>();
            errors.Add(response.ErrorCode switch
            {
                "UserDoesNotHaveEnoughBalance" => ErrorCode.INSUFFICIENT_FUNDS,
                _ => ErrorCode.BAD_REQUEST
            });
            return errors;
        }
    }
}
