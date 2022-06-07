using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Models
{
    public class Device
    {
        public int? DeviceId { get; set; }
        public string AgentId { get; set; }
        public bool? Enabled { get; set; }
        public string Name { get; set; }
        public string DeviceCode { get; set; }
        public DateTime? DeviceCodeIssuedAt { get; set; }
        public int? DeviceCodeExpiresInMin { get; set; }
        public string Token { get; set; }
        public DateTime? TokenIssuedAt { get; set; }
        public DateTime? TokenExpiresAt { get; set; }
    }
}
