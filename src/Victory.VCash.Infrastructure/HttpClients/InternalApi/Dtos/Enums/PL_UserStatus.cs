using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Enums
{
    //Platform User status
    public enum PL_UserStatus
    {
        DEL,
        CLSD,
        REG,
        ACT,
        INACT,
        UNCONF,
        SUSP,
        CONF,
        PCHANG,
        RTERMS,
        BLOCK,
        AWAITDEP,
        LCKD
    }
}
