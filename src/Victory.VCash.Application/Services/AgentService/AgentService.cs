using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Application.Services.AgentService.Inputs;
using Victory.VCash.Application.Services.AgentService.Results;
using Victory.VCash.Domain.Enums;
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
        IMapper _mapper;

        public AgentService(ILogger<AgentService> logger,
                            IPlatformWebSiteApiClient platformWebSiteApiClient,
                            IInternalApiClient internalApiClient,
                            IUnitOfWork unitOfWork,
                            IMapper mapper)
        {
            _platformWebSiteApiClient = platformWebSiteApiClient;
            _internalApiClient = internalApiClient;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RegisterAgentResult> RegisterAgentAsync(RegisterAgentInput input)
        {
            
            using (_unitOfWork.BeginTransaction())
            {
                var repository = _unitOfWork.GetRepository<AgentRepository>();
                //var company = repository.SaveCompany(input.Company);

                //input.Venues.ForEach(venue => venue.CompanyId = company.CompanyId);
                //input.Venues.ForEach(venue => venue = repository.SaveVenue(venue));

                //input.Agent.CompanyId = company.CompanyId;
                input.Agent = repository.SaveAgent(input.Agent);

                _unitOfWork.Commit();
            }

            try
            {
                var request = _mapper.Map<RegisterUserRequest>(input);
                var response = await _platformWebSiteApiClient.RegisterUserAsync(request);
                response.ThrowIfNotSuccess();

                input.Agent.UserId = int.Parse(response.Result.UserId);
                input.Agent = _unitOfWork.GetRepository<AgentRepository>().SaveAgent(input.Agent);

                return new RegisterAgentResult()
                {
                    Agent = input.Agent
                };
            }catch(Exception ex)
            {
                input.Agent.AgentStatusId = AgentStatus.ERROR;
                input.Agent.Error = ex.Message;
                _unitOfWork.GetRepository<AgentRepository>().SaveAgent(input.Agent);
                throw;
            }
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