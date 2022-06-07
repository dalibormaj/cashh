using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Enums
{
    public enum AgentStatus : int
    {
        PENDING_APPROVAL = 10,
        ACTIVE = 20,
        SUSPENDED = 30,
        DELETED = 40,
        ERROR = 100
    }
}
