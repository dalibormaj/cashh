using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Models
{
    public class Agent
    {
        public string AgentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CompanyId { get; set; }
        public string RefferalCode { get; set; }
    }
}
