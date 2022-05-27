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
    public class CashierRepository : Repository, ICashierRepository
    {
        private ICacheContext _cacheContext;

        public CashierRepository(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
        {
            _cacheContext = cacheContext;
        }

        public Cashier SaveCashier(Cashier cashier)
        {
            var s_cashier_id = !string.IsNullOrEmpty(cashier.CashierId) ? $"'{cashier.CashierId}'" : $"'{Guid.NewGuid()}'";
            var s_parentAgentId = !string.IsNullOrEmpty(cashier.ParentAgentId) ? $"'{cashier.ParentAgentId}'" : "null";
            var s_userName = !string.IsNullOrEmpty(cashier.UserName) ? $"'{cashier.UserName}'" : "null";
            var s_name = !string.IsNullOrEmpty(cashier.Name) ? $"'{cashier.Name}'" : "null";
            var s_lastName = !string.IsNullOrEmpty(cashier.LastName) ? $"'{cashier.LastName}'" : "null";
            var s_pin = !string.IsNullOrEmpty(cashier.Pin) ? $"'{cashier.Pin}'" : "null";

            var sql = $@"DO $$
                         DECLARE
	                         _cashier_id UUID := {s_cashier_id};
                             _parent_agent_id UUID := {s_parentAgentId};
	                         _shop_id INT := {cashier.ShopId};
	                         _user_name VARCHAR(100) := {s_userName};
	                         _name VARCHAR(100) := {s_name}; 
	                         _last_name VARCHAR(100) := {s_lastName};
	                         _pin VARCHAR(100) := {s_pin};
                         BEGIN 
	                         IF EXISTS(SELECT 'x' FROM cashier WHERE cashier_id = _cashier_id) THEN
		                        UPDATE cashier SET parent_agent_id = _parent_agent_id, 
						                           shop_id = _shop_id, 
						                           user_name = _user_name, 
						                           name = _name, 
						                           last_name = _last_name, 
						                           pin = _pin
		                        WHERE cashier_id = _cashier_id;
	                         ELSE
                                INSERT INTO public.cashier(cashier_id, parent_agent_id, shop_id, user_name, name, last_name, pin) 
		                        VALUES (_cashier_id, _parent_agent_id, _shop_id, _user_name, _name, _last_name, _pin);
	                         END IF;

	                         -- create temp table with affected rows
	                         CREATE TEMPORARY TABLE _tmp{nameof(Cashier)} ON COMMIT DROP 
	                         AS
	                         SELECT * 
	                         FROM cashier 
	                         WHERE cashier_id = _cashier_id;
                         END $$;

                         -- result
                         SELECT * 
                         FROM _tmp{nameof(Cashier)}"; 

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Cashier>()
                   .ForMember(d => d.CashierId, opt => opt.MapFrom(src => src["cashier_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.ShopId, opt => opt.MapFrom(src => src["shop_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
                   .ForMember(d => d.Pin, opt => opt.MapFrom(src => src["pin"]));
            }).CreateMapper();

            var result = DataContext.ExecuteSql<Cashier>(sql, mapper).FirstOrDefault();
            return result;
        }

        public Cashier GetCashier(string cashierId)
        {
            var sql = $@"SELECT cashier_id, parent_agent_id, shop_id, user_name, name, last_name, pin
                         FROM public.cashier
                         WHERE cashier_id = '{cashierId}'";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Cashier>()
                   .ForMember(d => d.CashierId, opt => opt.MapFrom(src => src["cashier_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.ShopId, opt => opt.MapFrom(src => src["shop_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
                   .ForMember(d => d.Pin, opt => opt.MapFrom(src => src["pin"]));
            }).CreateMapper();

            var cashier = DataContext.ExecuteSql<Cashier>(sql, mapper).SingleOrDefault();
            return cashier;
        }

        public Cashier GetCashier(string cashier_id = "", string parentAgentId = "", string userName = "", int? companyId = null, int? shopId = null)
        {
            string s_cashier_id = !string.IsNullOrEmpty(cashier_id) ? $"'{cashier_id}'" : "null";
            string s_parent_agent_id = !string.IsNullOrEmpty(parentAgentId) ? $"'{parentAgentId}'" : "null";
            string s_userName = !string.IsNullOrEmpty(userName) ? $"'{userName}'" : "null";
            string s_company_id = companyId?.ToString()?? "null";
            string s_shop_id = shopId?.ToString() ?? "null";

            var sql = $@"SELECT c.cashier_id, c.parent_agent_id, c.shop_id, c.user_name, c.name, c.last_name, c.pin
                         FROM public.cashier c
                         JOIN public.shop s ON s.shop_id = c.shop_id
                         WHERE c.cashier_id = COALESCE({s_cashier_id}, c.cashier_id)
                           AND c.parent_agent_id = COALESCE({s_parent_agent_id}, c.parent_agent_id)
                           AND c.user_name = COALESCE({s_userName}, c.user_name)
                           AND c.shop_id = COALESCE({s_shop_id}, c.shop_id)
                           AND s.company_id = COALESCE({s_company_id}, s.company_id)";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Cashier>()
                   .ForMember(d => d.CashierId, opt => opt.MapFrom(src => src["cashier_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.ShopId, opt => opt.MapFrom(src => src["shop_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
                   .ForMember(d => d.Pin, opt => opt.MapFrom(src => src["pin"]));
            }).CreateMapper();

            var cashier = DataContext.ExecuteSql<Cashier>(sql, mapper).SingleOrDefault();
            return cashier;
        }

    }
}
