using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.HttpClients.InternalApi;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests;
using Victory.VCash.Infrastructure.Repositories;

namespace Victory.VCash.Application.Services.AgentService
{
    public class AgentService : IAgentService
    {
        IPlatformWebSiteApiClient _platformWebSiteApiClient;
        IInternalApiClient _internalApiClient;
        ILogger<AgentService> _logger;
        IUnitOfWork _unitOfWork;

        public AgentService(ILogger<AgentService> logger,
                            IPlatformWebSiteApiClient platformWebSiteApiClient,
                            IInternalApiClient internalApiClient,
                            IUnitOfWork unitOfWork)
        {
            _platformWebSiteApiClient = platformWebSiteApiClient;
            _internalApiClient = internalApiClient;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public void Register()
        {
            throw new NotImplementedException();
        }

        public async Task RequestPasswordResetAsync(string identifier, string passwordResetUrl)
        {
            var response = await _platformWebSiteApiClient.RequestPasswordResetAsync(new RequestPasswordResetRequest()
            {
                UserName = identifier,
                PasswordResetPageUrl = passwordResetUrl
            });

            if (response.ResponseCode != 0)
                throw new VCashException(ErrorCode.AGENT_DOES_NOT_EXIST);
        }

        public async Task CompletePasswordResetAsync(string newPassword, string token)
        {
            var response = await _platformWebSiteApiClient.CompletePasswordResetAsync(new CompletePasswordResetRequest() { NewPassword = newPassword, ResetToken = token });
            if (response.ResponseCode != 0)
                throw new VCashException(ErrorCode.PASSWORD_RESET_CANNOT_COMPLETE);
        }

        public Agent GetAgent(string agentId)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_DOES_NOT_EXIST);

            return agent;
        }

        public Agent GetAgent(int userId)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(userId: userId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_DOES_NOT_EXIST);

            return agent;
        }
    }
}
