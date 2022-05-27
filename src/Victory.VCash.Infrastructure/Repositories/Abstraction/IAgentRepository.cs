using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Infrastructure.Repositories.Abstraction
{
    public interface IAgentRepository
    {
        Agent GetAgent(string agentId);
    }
}
