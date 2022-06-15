using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Application.Services.AgentService.Inputs;
using Victory.VCash.Application.Services.AgentService.Results;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Common;
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
        HttpContext _httpContext;
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

        public async Task OverridePasswordAsync(Guid agentId, string newPassword, string guardianToken)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            if(agent.UserId == null)
                throw new VCashException(ErrorCode.PL_USER_CANNOT_BE_FOUND);

            var response = await _platformWebSiteApiClient.OverridePasswordAsync(new OverridePasswordRequest()
            {
                NewPassword = newPassword
            }, guardianToken);
        }

        public async Task<RegisterAgentResult> RegisterAgentAsync(RegisterAgentInput input)
        {
            if (input.Agent == null)
                throw new VCashException(ErrorCode.AGENT_REGISTRATION_FAILED);

            if (input.Agent.ParentAgentId == null || string.IsNullOrEmpty(input.Agent.Email))
                throw new VCashException(ErrorCode.AGENT_REGISTRATION_FAILED);

            if (input.Company == null)
                throw new VCashException(ErrorCode.AGENT_REGISTRATION_FAILED);

            using (_unitOfWork.BeginTransaction())
            {
                var repository = _unitOfWork.GetRepository<AgentRepository>();

                //check the parent agent status
                var parentAgent = repository.GetAgent(input.Agent.ParentAgentId.Value);
                if (parentAgent.AgentStatusId != AgentStatus.ACTIVE)
                    throw new VCashException(ErrorCode.PARENT_AGENT_IS_NOT_ACTIVE);

                //check if agent with the same email already exists
                var agents = repository.GetAgents(email: input.Agent.Email);
                if (agents.Count() > 1)
                    throw new VCashException(ErrorCode.AGENT_REGISTRATION_FAILED);

                var existing_agent = agents.FirstOrDefault();
                if (existing_agent != null)
                {
                    //only invalid agents can be adjusted
                    if (existing_agent.AgentStatusId != AgentStatus.ERROR)
                        throw new VCashException(ErrorCode.AGENT_ALREADY_EXISTS);

                    //refresh existing agent values
                    input.Agent = existing_agent?.ApplyNewValues(input.Agent);
                    input.Agent.Error = string.Empty; //remove error message
                }

                if (input.Company != null)
                {
                    //check if company with the same tax number already exists
                    var companies = repository.GetCompanies(taxNumber: input.Company.TaxNumber);
                    if (companies.Count() > 1)
                        throw new VCashException(ErrorCode.MORE_COMPANIES_ALREADY_REGISTED_WITH_THE_SAME_TAX_NUMBER);

                    var existingCompany = companies?.FirstOrDefault();

                    //save company
                    //refresh existing company values or add a new one if not exists
                    input.Company = existingCompany?.ApplyNewValues(input.Company) ?? input.Company;
                    input.Company = repository.SaveCompany(input.Company);


                    //save venues
                    //append existing values 
                    var existingVenues = repository.GetVenues(input.Company.CompanyId.Value) ?? new List<Venue>();
                    existingVenues.AddRange(input.Venues.Where(venue => !existingVenues.Exists(x => x.Name == venue.Name)));
                    input.Venues = existingVenues;
                    input.Venues?.ForEach(venue => 
                    { 
                        venue.CompanyId = venue.CompanyId ?? input.Company.CompanyId;
                        repository.SaveVenue(venue);
                    });


                    //save bank accounts
                    //append existing values 
                    input.BankAccounts.ForEach(bankAccount => 
                    {
                        BankAccountHelper.TryFormat(bankAccount.AccountNumber, out string formatted, withDashes:true);
                        bankAccount.AccountNumber = formatted;
                    });
                    var existingBankAccounts = repository.GetBankAccounts(companyId: input.Company.CompanyId);

                    existingBankAccounts.AddRange(input.BankAccounts.Where(bankAccount => !existingBankAccounts.Exists(x =>
                    {
                        BankAccountHelper.TryFormat(bankAccount.AccountNumber, out string existingAccount);
                        BankAccountHelper.TryFormat(x.AccountNumber, out string newAccount);
                        return existingAccount == newAccount;
                    })));
                    input.BankAccounts = existingBankAccounts;
                    input.BankAccounts?.ForEach(bankAccount =>
                    {
                        bankAccount.CompanyId = bankAccount.CompanyId ?? input.Company.CompanyId;
                        bankAccount.Bank = BankAccountHelper.GetBankName(bankAccount.AccountNumber);
                        bankAccount.AccountNumber = bankAccount.AccountNumber;
                        repository.SaveBankAccount(bankAccount);
                    });

                    input.Agent.CompanyId = input.Company.CompanyId;
                }

                input.Agent = repository.SaveAgent(input.Agent);

                _unitOfWork.Commit();
            }

            return new RegisterAgentResult()
            {
                Agent = input.Agent,
                Company = input.Company,
                Venues = input.Venues
            };
        }

        public async Task<RegisterAgentResult> RegisterMasterAgentAsync(string userName, string email, string firstName, string lastName)
        {
            var masterCompanyId = 1;//TODO!!! FIX THIS
            var repository = _unitOfWork.GetRepository<AgentRepository>();

            var masterCompany = repository.GetCompany(masterCompanyId);
            if (masterCompany == null)
                throw new VCashException(ErrorCode.COMPANY_CANNOT_BE_FOUND);

            //check if agent with the same email already exists
            var agents = repository.GetAgents(email: email);
            if (agents.Count() > 1)
                throw new VCashException(ErrorCode.AGENT_REGISTRATION_FAILED);

            var existing_agent = agents.FirstOrDefault();

            var newAgent = new Agent()
            {
                AgentStatusId = AgentStatus.ACTIVE,
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                CompanyId = masterCompanyId
            };

            if (existing_agent != null)
            {
                //only invalid agents can be adjusted
                if (existing_agent.AgentStatusId != AgentStatus.ERROR)
                    throw new VCashException(ErrorCode.AGENT_ALREADY_EXISTS);

                //refresh existing agent values.
                newAgent = existing_agent?.ApplyNewValues(newAgent);
                newAgent.Error = string.Empty; //remove error message
            }

            newAgent = repository.SaveAgent(newAgent);
            return new RegisterAgentResult()
            {
                Agent = newAgent,
                Company = masterCompany
            };
        }

        public async Task RequestPasswordResetAsync(string identifier, string passwordResetUrl)
        {
            var response = await _platformWebSiteApiClient.RequestPasswordResetAsync(new RequestPasswordResetRequest()
            {
                UserName = identifier,
                PasswordResetPageUrl = passwordResetUrl
            });

            if (response.ResponseCode != 0)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);
        }

        public async Task CompletePasswordResetAsync(string newPassword, string token)
        {
            var response = await _platformWebSiteApiClient.CompletePasswordResetAsync(new CompletePasswordResetRequest() { NewPassword = newPassword, ResetToken = token });
            if (response.ResponseCode != 0)
                throw new VCashException(ErrorCode.PASSWORD_RESET_CANNOT_COMPLETE);
        }

        public Agent GetAgent(Guid agentId)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            return agent;
        }

        public Agent GetAgent(int userId)
        {
            var agents = _unitOfWork.GetRepository<AgentRepository>().GetAgents(userId: userId);
            if (agents == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            if (agents.Count() > 1)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            return agents.FirstOrDefault();
        }

        public Agent GetMasterAgent(string email)
        {
            var masterCompanyId = 1;
            var agents = _unitOfWork.GetRepository<AgentRepository>().GetAgents(email: email, companyId: masterCompanyId);
            if ((agents?.Count() ?? 0) == 0)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND, $"[{email}]");

            if(agents.Count() > 1)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND, $"[{email}]");

            return agents.FirstOrDefault();
        }

        public async Task<RegisterAgentResult> ActivateAgentAsync(Guid agentId, string emailVerificationUrl)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            try
            {
                if (agent.AgentStatusId != AgentStatus.ACTIVE && agent.UserId == null)
                {
                    var request = _mapper.Map<RegisterUserRequest>(agent);
                    request.EmailVerificationUrl = emailVerificationUrl;
                    var registerUserResponse = await _platformWebSiteApiClient.RegisterUserAsync(request);
                    registerUserResponse.ThrowIfNotSuccess();

                    var emailVerficationResponse = await _platformWebSiteApiClient.SendEmailVerificationCodeAsync(
                        new SendEmailVerificationCodeRequest()
                        {
                            UserId = registerUserResponse.Result.UserId
                        });
                    emailVerficationResponse.ThrowIfNotSuccess();

                    agent.AgentStatusId = AgentStatus.PENDING_VERIFICATION;
                    agent.UserId = int.Parse(registerUserResponse.Result.UserId);
                    agent.UserName = registerUserResponse.Result.UserName;                  
                    agent = _unitOfWork.GetRepository<AgentRepository>().SaveAgent(agent);
                }

                var company = _unitOfWork.GetRepository<AgentRepository>().GetCompany(agent.CompanyId.Value);
                var venues = _unitOfWork.GetRepository<AgentRepository>().GetVenues(agent.CompanyId.Value);

                return new RegisterAgentResult()
                {
                    Agent = agent,
                    Company = company,
                    Venues = venues
                };

            }
            catch (Exception ex)
            {
                agent.AgentStatusId = AgentStatus.ERROR;
                agent.Error = ex.Message;
                _unitOfWork.GetRepository<AgentRepository>().SaveAgent(agent);
                throw;
            }
        }

        public List<Venue> GetVenues(int companyId)
        {
            return _unitOfWork.GetRepository<AgentRepository>().GetVenues(companyId);
        }

        public Venue GetVenue(int venueId)
        {
            return _unitOfWork.GetRepository<AgentRepository>().GetVenue(venueId);
        }

        public async Task SendEmailVerificationCode(Guid agentId)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            if(agent.UserId == null)
                throw new VCashException(ErrorCode.PL_USER_CANNOT_BE_FOUND);

            var emailVerficationResponse = await _platformWebSiteApiClient.SendEmailVerificationCodeAsync(
                        new SendEmailVerificationCodeRequest()
                        {
                            UserId = agent.UserId.ToString()
                        });
            emailVerficationResponse.ThrowIfNotSuccess();
        }

        public async Task SendSmsVerificationCode(Guid agentId)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            if (agent.UserId == null)
                throw new VCashException(ErrorCode.PL_USER_CANNOT_BE_FOUND);

            if(agent.AgentStatusId != AgentStatus.PENDING_VERIFICATION)
                throw new VCashException(ErrorCode.AGENT_IS_NOT_IN_STATUS_PENDING_VERIFICATION);

            var response = await _platformWebSiteApiClient.SendSmsVerificationCodeAsync(
                        new SendSmsVerificationCodeRequest()
                        {
                            UserId = agent.UserId.ToString()
                        });
            response.ThrowIfNotSuccess();
        }

        public async Task VerifyEmail(Guid agentId, string verificationCode)
        {
            var agent = _unitOfWork.GetRepository<AgentRepository>().GetAgent(agentId);
            if (agent == null)
                throw new VCashException(ErrorCode.AGENT_CANNOT_BE_FOUND);

            if (agent.UserId == null)
                throw new VCashException(ErrorCode.PL_USER_CANNOT_BE_FOUND);

            var response = await _platformWebSiteApiClient.VerifyEmailAsync(
                        new VerifyEmailRequest()
                        {
                           UserId = agent.UserId.ToString(),
                           VerificationCode = verificationCode
                        });
            response.ThrowIfNotSuccess();
        }

        public Task VerifySms(Guid agentId, string verificationCode)
        {
            throw new NotImplementedException();
        }
    }
}