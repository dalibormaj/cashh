using System.Threading.Tasks;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Application.Services.MoneyTransferService
{
    public interface IMoneyTransferProvider
    {
        MoneyTransfer Create(int fromUserId, int toUserId, decimal amount);
        Task<MoneyTransfer> ProcessAsync(long moneyTransferId, MoneyTransferStatus? newStatus = null);
    }

}
