using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Application.Services.AuthService.Results
{
    public class ValidateDeviceTokenResult
    {
        public bool IsValid { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }  
        public string AgentId { get; set; }
        public DateTime? IssuedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public List<Claim> Claims { get; set; }
        public List<Error> Errors { get; set; }
    }
}
