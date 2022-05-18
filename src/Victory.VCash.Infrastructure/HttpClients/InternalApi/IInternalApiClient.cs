using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Requests;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi
{
    public interface IInternalApiClient
    {
        Task<GetUserDetailsResponse> GetUserDetailsAsync(GetUserDetailsRequest request);
        Task<CreateTransactionResponse> CreateTransactionAsync(CreateTransactionRequest request);
        Task<UpdateTransactionResponse> UpdateTransactionAsync(UpdateTransactionRequest request);
    }
}
