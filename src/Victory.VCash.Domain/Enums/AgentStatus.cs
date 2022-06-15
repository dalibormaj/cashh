using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Enums
{
    public enum AgentStatus : int
    {
        DRAFT = 10,
        PENDING_VERIFICATION = 20,
        ACTIVE = 30,
        BLOCKED = 40,
        SUSPENDED = 50,
        ERROR = 100
    }
}
