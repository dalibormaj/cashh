using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Repositories.Abstraction;

namespace Victory.VCash.Infrastructure.Repositories
{
    public class AgentRepository : Repository, IAgentRepository
    {
        private ICacheContext _cacheContext;

        public AgentRepository(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
        {
            _cacheContext = cacheContext;
        }

        public Agent GetAgent(string agentId)
        {
            var sql = $@"SELECT agent_id, user_id, user_name, company_id, refferal_code
                         FROM public.agent
                         WHERE agent_id = '{agentId}'";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Agent>()
                   .ForMember(d => d.AgentId, opt => opt.MapFrom(src => src["agent_id"]))
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.RefferalCode, opt => opt.MapFrom(src => src["refferal_code"]));
            }).CreateMapper();

            var agent = DataContext.ExecuteSql<Agent>(sql, mapper).SingleOrDefault();
            return agent;
        }

        public Agent GetAgent(string agentId = null, int? userId = null, string userName = null)
        {
            string s_agentId = agentId != null? $"'{agentId}'" : "null";
            string s_userId = userId?.ToString() ?? "null";
            string s_userName = userName != null ? $"'{userName}'" : "null";

            var sql = $@"SELECT agent_id, user_id, user_name, company_id, refferal_code
                         FROM public.agent a
                         WHERE a.agent_id = COALESCE({s_agentId}, a.agent_id)
                           AND a.user_id = COALESCE({s_userId}, a.user_id)
                           AND COALESCE(a.user_name, '') = COALESCE({s_userName}, a.user_name, '')";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Agent>()
                   .ForMember(d => d.AgentId, opt => opt.MapFrom(src => src["agent_id"]))
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.RefferalCode, opt => opt.MapFrom(src => src["refferal_code"]));
            }).CreateMapper();

            var agent = DataContext.ExecuteSql<Agent>(sql, mapper).SingleOrDefault();
            return agent;
        }

        public Company GetCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public Company GetCompany(string agentId)
        {
            throw new NotImplementedException();
        }

        public Venue GetVenue(int venueId)
        {
            throw new NotImplementedException();
        }

        public List<Venue> GetVenues(int companyId)
        {
            throw new NotImplementedException();
        }

        public Agent SaveAgent(Agent agent)
        {
            var s_agent_id = !string.IsNullOrEmpty(agent.AgentId?.ToString()) ? $"'{agent.AgentId}'" : $"'{Guid.NewGuid()}'";
            var s_userId = agent.UserId?.ToString() ?? "null";
            var s_agentStatusId = ((int?)agent.AgentStatusId)?.ToString() ?? "null";
            var s_parentAgentId = !string.IsNullOrEmpty(agent.ParentAgentId?.ToString()) ? $"'{agent.ParentAgentId}'" : "null";
            var s_userName = !string.IsNullOrEmpty(agent.UserName) ? $"'{agent.UserName}'" : "null";
            var s_email = !string.IsNullOrEmpty(agent.Email) ? $"'{agent.Email}'" : "null";
            var s_phoneNumber  = !string.IsNullOrEmpty(agent.PhoneNumber) ? $"'{agent.PhoneNumber}'" : "null";
            var s_companyId = agent.CompanyId?.ToString() ?? "null";
            var s_refferalCode = !string.IsNullOrEmpty(agent.RefferalCode) ? $"'{agent.RefferalCode}'" : "null";
            var s_error = !string.IsNullOrEmpty(agent.Error) ? $"'{agent.Error}'" : "null";

            var sql = $@"DO $$
                         DECLARE
	                         _agent_id UUID := {s_agent_id};
                             _user_id INT := {s_userId};
                             _agent_status_id INT := {s_agentStatusId};
                             _parent_agent_id UUID := {s_parentAgentId};
	                         _user_name VARCHAR(100) := {s_userName};
                             _email VARCHAR(200) := {s_email};
                             _phone_number VARCHAR(100) := {s_phoneNumber};
	                         _company_id INT := {s_companyId};
	                         _refferal_code VARCHAR(100) := {s_refferalCode}; 
                             _error TEXT := {s_error};
                             _now TIMESTAMP := NOW() AT time zone 'utc';
                         BEGIN 
	                         IF EXISTS(SELECT 'x' FROM agent WHERE agent_id = _agent_id) THEN
		                        UPDATE agent SET user_id = _user_id,
						                         agent_status_id = _agent_status_id,
						                         parent_agent_id = _parent_agent_id,
						                         user_name = _user_name,
                                                 email = _email,
                                                 phone_number = _phone_number,
						                         company_id = _company_id,
                                                 refferal_code = _refferal_code,
                                                 error = _error
		                        WHERE agent_id = _agent_id;
	                         ELSE
                                INSERT INTO public.agent(agent_id, user_id, agent_status_id, parent_agent_id, user_name, email, phone_number, company_id, refferal_code, error, insert_date) 
		                        VALUES (_agent_id, _user_id, _agent_status_id, _parent_agent_id, _user_name, _email, _phone_number, _company_id, _refferal_code, _error, _now);
	                         END IF;

	                         -- create temp table with affected rows
	                         CREATE TEMPORARY TABLE _tmp{nameof(Agent)} ON COMMIT DROP 
	                         AS
	                         SELECT * 
	                         FROM agent 
	                         WHERE agent_id = _agent_id;
                         END $$;

                         -- result
                         SELECT * 
                         FROM _tmp{nameof(Agent)}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Agent>()
                   .ForMember(d => d.AgentId, opt => opt.MapFrom(src => src["agent_id"]))
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.AgentStatusId, opt => opt.MapFrom(src => src["agent_status_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.Email, opt => opt.MapFrom(src => src["email"]))
                   .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(src => src["phone_number"]))
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.RefferalCode, opt => opt.MapFrom(src => src["refferal_code"]))
                   .ForMember(d => d.Error, opt => opt.MapFrom(src => src["error"]));
            }).CreateMapper();

            var result = DataContext.ExecuteSql<Agent>(sql, mapper).FirstOrDefault();
            return result;
        }

        public Company SaveCompany(Company company)
        {
            throw new NotImplementedException();
        }

        public Venue SaveVenue(Venue venue)
        {
            throw new NotImplementedException();
        }
    }
}
