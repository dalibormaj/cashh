using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Application.Services.AuthService.Results
{
    public class CreateDeviceTokenResult
    {
        public string DeviceToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
