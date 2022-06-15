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
        Task<RegisterAgentResult> RegisterMasterAgentAsync(string userName, string email, string firstName, string lastName);
        Task<RegisterAgentResult> ActivateAgentAsync(Guid agentId, string emailVerificationUrl);
        Task SendEmailVerificationCode(Guid agentId);
        Task SendSmsVerificationCode(Guid agentId);
        Task VerifyEmail(Guid agentId, string verificationCode);
        Task VerifySms(Guid agentId, string verificationCode);
        Task OverridePasswordAsync(Guid agentId, string newPassword, string guardianToken);
        Agent GetAgent(Guid agentId);
        Agent GetAgent(int userId);
        Agent GetMasterAgent(string email);
        Venue GetVenue(int venueId);
        List<Venue> GetVenues(int companyId);
        Task RequestPasswordResetAsync(string identifier, string passwordResetUrl);
        Task CompletePasswordResetAsync(string newPassword, string token);
    }
}
