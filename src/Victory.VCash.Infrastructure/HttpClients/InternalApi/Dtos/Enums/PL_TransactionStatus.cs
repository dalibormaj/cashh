using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Enums
{
    //Platform transaction type
    public enum PL_TransactionStatus
    {
        PENDING	= 100,
        PROCESSING = 200,
        ONHOLD = 300,
        VIRTUAL	= 400,
        COMPLETE = 500,
        FAILED = 600,
        VOID = 700,
        REJECTED = 800,
        REAL = 900,
        APPROVED = 1000,
        AWAITING_CASHOUT = 1100,
        VALIDATED = 1200,
        CANCELLED = 1300,
        QUEUED = 1400,
        MANUAL_PROCESSING = 1500,
        AWAITING_INFO = 1600
    }
}
