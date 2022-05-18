using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Enums
{
    //Platform transaction type
    public enum PL_TransactionType
    {
        REFUND_AGENT_CASH_PAYOUT = 4366,
        REFUND_AGENT_CASH_DEPOSIT = 4367,
        REFUND_USER_CASH_PAYOUT	= 4368,
        REFUND_USER_CASH_DEPOSIT = 4369,
        AGENT_CASH_PAYOUT = 4370,
        AGENT_CASH_DEPOSIT = 4371,
        USER_CASH_PAYOUT = 4372,
        USER_CASH_DEPOSIT = 4373
    }
}
