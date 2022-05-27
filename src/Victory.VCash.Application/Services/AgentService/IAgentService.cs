using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Application.Services.AgentService
{
    public interface IAgentService
    {
        void Register();
        Agent GetAgent(string agentId);
        Agent GetAgent(int userId);
        Task RequestPasswordResetAsync(string identifier, string passwordResetUrl);
        Task CompletePasswordResetAsync(string newPassword, string token);
    }
}
