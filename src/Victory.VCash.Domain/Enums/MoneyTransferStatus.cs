using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Enums
{
    public enum MoneyTransferStatus
    {
        PENDING_APPROVAL = 10,
        APPROVED = 20,
        REJECTED = 30,
        COMPLETED = 40,
        REFUNDED = 50,
        ERROR = 100
    }
}
