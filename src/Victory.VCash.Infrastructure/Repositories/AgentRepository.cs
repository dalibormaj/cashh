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

        public Agent GetAgent(string agentId = "", int? userId = null, string userName = "")
        {
            string s_agentId = !string.IsNullOrEmpty(agentId)? $"'{agentId}'" : "null";
            string s_userId = userId?.ToString() ?? "null";
            string s_userName = !string.IsNullOrEmpty(userName) ? $"'{userName}'" : "null";

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
    }
}
