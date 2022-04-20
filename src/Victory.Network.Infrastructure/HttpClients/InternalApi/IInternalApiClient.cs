using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Requests;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.Network.Infrastructure.HttpClients.InternalApi
{
    public interface IInternalApiClient
    {
        Task<GetUserDetailsResponse> GetUserDetails(GetUserDetailsRequest request);
        Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request);
    }
}
