using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Application.Services.AgentService.Inputs
{
    public class RegisterAgentInput
    {  
        public Agent Agent { get; set; }
        public Company Company { get; set; }
        public List<Venue> Venues { get ; set; }
        public List<BankAccount> BankAccounts { get; set; }
    }
}
