using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.Repositories.Settings
{
    public class AuthServiceSettings
    {
        public DeviceTokenSection DeviceToken { get; set; }
        public CashierTokenSection CashierToken { get; set; }
    }

    public class DeviceTokenSection
    {
        public int DeviceCodeExpiresInMin { get; set; }
        public int ExpiresInYears { get; set; }
        public string KeyId { get; set; }
        public string SecurityKey { get; set; }
    }

    public class CashierTokenSection
    {
        public string KeyId { get; set; }
        public string SecurityKey { get; set; }
    }
}
