using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Application.Services.AgentService.Inputs;
using Victory.VCash.Application.Services.AgentService.Results;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Application.Services.AgentService
{
    public interface IAgentService
    {
        Task<RegisterAgentResult> RegisterAgentAsync(RegisterAgentInput input);
        Agent GetAgent(string agentId);
        Agent GetAgent(int userId);
        Task RequestPasswordResetAsync(string identifier, string passwordResetUrl);
        Task CompletePasswordResetAsync(string newPassword, string token);
    }
}
