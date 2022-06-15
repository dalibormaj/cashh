using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Enums;

namespace Victory.VCash.Domain.Models
{
    public class Agent : BaseDomainModel
    {
        public Guid? AgentId { get; set; }
        public AgentStatus? AgentStatusId { get; set; }
        public Guid? ParentAgentId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string AdUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? CompanyId { get; set; }
        public string RefferalCode { get; set; }
        public string Error { get; set; }
    }
}
