using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Domain.Models;
using Victory.VCash.Domain.Query;
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
            var s_parentAgentId = cashier.ParentAgentId != null ? $"'{cashier.ParentAgentId}'" : "null";
            var s_venueId = cashier.VenueId.HasValue? cashier.VenueId.Value.ToString() : "null";
            var s_cashierStatusId = ((int?)cashier.CashierStatusId)?.ToString() ?? "null";
            var s_userName = !string.IsNullOrEmpty(cashier.UserName) ? $"'{cashier.UserName}'" : "null";
            var s_FirstName = !string.IsNullOrEmpty(cashier.FirstName) ? $"'{cashier.FirstName}'" : "null";
            var s_lastName = !string.IsNullOrEmpty(cashier.LastName) ? $"'{cashier.LastName}'" : "null";
            var s_pin = !string.IsNullOrEmpty(cashier.Pin) ? $"'{cashier.Pin}'" : "null";
            var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var sql = $@"DO $$
                         DECLARE
	                         _cashier_id UUID := {s_cashier_id};
                             _parent_agent_id UUID := {s_parentAgentId};
	                         _venue_id INT := {s_venueId};
                             _cashier_status_id INT := {s_cashierStatusId};
	                         _user_name VARCHAR(100) := {s_userName};
	                         _first_name VARCHAR(100) := {s_FirstName}; 
	                         _last_name VARCHAR(100) := {s_lastName};
	                         _pin VARCHAR(100) := {s_pin};
                         BEGIN 
	                         IF EXISTS(SELECT 'x' FROM cashier WHERE cashier_id = _cashier_id) THEN
		                        UPDATE cashier SET parent_agent_id = _parent_agent_id, 
						                           venue_id = _venue_id, 
                                                   cashier_status_id = _cashier_status_id,
						                           user_name = _user_name, 
						                           first_name = _first_name, 
						                           last_name = _last_name, 
						                           pin = _pin
		                        WHERE cashier_id = _cashier_id;
	                         ELSE
                                INSERT INTO public.cashier(cashier_id, parent_agent_id, venue_id, cashier_status_id, user_name, first_name, last_name, pin) 
		                        VALUES (_cashier_id, _parent_agent_id, _venue_id, _cashier_status_id, _user_name, _first_name, _last_name, _pin);
	                         END IF;

	                         -- create temp table with affected rows
	                         CREATE TEMPORARY TABLE _tmp{unixMs} ON COMMIT DROP 
	                         AS
	                         SELECT * 
	                         FROM cashier 
	                         WHERE cashier_id = _cashier_id;
                         END $$;

                         -- result
                         SELECT * 
                         FROM _tmp{unixMs}"; 

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Cashier>()
                   .ForMember(d => d.CashierId, opt => opt.MapFrom(src => src["cashier_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.VenueId, opt => opt.MapFrom(src => src["venue_id"]))
                   .ForMember(d => d.CashierStatusId, opt => opt.MapFrom(src => src["cashier_status_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src["first_name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
                   .ForMember(d => d.Pin, opt => opt.MapFrom(src => src["pin"]));
            }).CreateMapper();

            var result = DataContext.ExecuteSql<Cashier>(sql, mapper).FirstOrDefault();
            return result;
        }

        public Cashier GetCashier(Guid cashierId)
        {
            var sql = $@"SELECT cashier_id, parent_agent_id, venue_id, cashier_status_id, user_name, first_name, last_name, pin
                         FROM public.cashier
                         WHERE cashier_id = '{cashierId}'";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Cashier>()
                   .ForMember(d => d.CashierId, opt => opt.MapFrom(src => src["cashier_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.VenueId, opt => opt.MapFrom(src => src["venue_id"]))
                   .ForMember(d => d.CashierStatusId, opt => opt.MapFrom(src => src["cashier_status_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src["first_name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
                   .ForMember(d => d.Pin, opt => opt.MapFrom(src => src["pin"]));
            }).CreateMapper();

            var cashier = DataContext.ExecuteSql<Cashier>(sql, mapper).SingleOrDefault();
            return cashier;
        }

        public List<Cashier> GetCashiers(Guid? cashierId = null, Guid? parentAgentId = null, string userName = "", int? companyId = null, int? venueId = null)
        {
            string s_cashier_id = cashierId != null ? $"'{cashierId}'" : "null";
            string s_parent_agent_id = parentAgentId != null ? $"'{parentAgentId}'" : "null";
            string s_userName = !string.IsNullOrEmpty(userName) ? $"'{userName}'" : "null";
            string s_company_id = companyId?.ToString()?? "null";
            string s_venueId = venueId?.ToString() ?? "null";

            var sql = $@"SELECT c.cashier_id, c.parent_agent_id, c.venue_id, c.cashier_status_id, c.user_name, c.first_name, c.last_name, c.pin
                         FROM public.cashier c
                         JOIN public.Venue s ON s.venue_id = c.venue_id
                         WHERE c.cashier_id = COALESCE({s_cashier_id}, c.cashier_id)
                           AND c.parent_agent_id = COALESCE({s_parent_agent_id}, c.parent_agent_id)
                           AND c.user_name = COALESCE({s_userName}, c.user_name)
                           AND c.venue_id = COALESCE({s_venueId}, c.venue_id)
                           AND s.company_id = COALESCE({s_company_id}, s.company_id)";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Cashier>()
                   .ForMember(d => d.CashierId, opt => opt.MapFrom(src => src["cashier_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.VenueId, opt => opt.MapFrom(src => src["venue_id"]))
                   .ForMember(d => d.CashierStatusId, opt => opt.MapFrom(src => src["cashier_status_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src["first_name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
                   .ForMember(d => d.Pin, opt => opt.MapFrom(src => src["pin"]));
            }).CreateMapper();

            var cashiers = DataContext.ExecuteSql<Cashier>(sql, mapper);
            return cashiers;
        }

        public List<CashierWithDetails> GetAllCashiersWithDetails(Guid agentId)
        {
            var sql = $@"SELECT c.cashier_id, c.parent_agent_id, c.venue_id, c.cashier_status_id, c.user_name, c.first_name, c.last_name, c.pin,
	                            v.name as ""venue_name"",  v.company_id, v.city as ""venue_city"", v.municipality as ""venue_municipality"", v.address as ""venue_address"",
                                cc.name as ""company_name"", cc.registration_number as ""company_registration_number"", cc.tax_number as ""company_tax_number"", 
                                cc.city as ""company_city"", cc.municipality as ""company_municipality"", cc.address as ""company_address""
                         FROM public.cashier c
                         JOIN public.venue v ON v.venue_id = c.venue_id
                         JOIN public.company cc ON cc.company_id = v.company_id
                         WHERE c.parent_agent_id = '{agentId}'";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, CashierWithDetails>()
                   .ForMember(d => d.CashierId, opt => opt.MapFrom(src => src["cashier_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.VenueId, opt => opt.MapFrom(src => src["venue_id"]))
                   .ForMember(d => d.CashierStatusId, opt => opt.MapFrom(src => src["cashier_status_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src["first_name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
                   .ForMember(d => d.Pin, opt => opt.MapFrom(src => src["pin"]))

                   .ForMember(d => d.VenueName, opt => opt.MapFrom(src => src["venue_name"]))
                   .ForMember(d => d.VenueCity, opt => opt.MapFrom(src => src["venue_city"]))
                   .ForMember(d => d.VenueMunicipality, opt => opt.MapFrom(src => src["venue_municipality"]))
                   .ForMember(d => d.VenueAddress, opt => opt.MapFrom(src => src["venue_address"]))

                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.CompanyName, opt => opt.MapFrom(src => src["company_name"]))
                   .ForMember(d => d.CompanyRegistrationNumber, opt => opt.MapFrom(src => src["company_registration_number"]))
                   .ForMember(d => d.CompanyTaxNumber, opt => opt.MapFrom(src => src["company_tax_number"]))
                   .ForMember(d => d.CompanyCity, opt => opt.MapFrom(src => src["company_city"]))
                   .ForMember(d => d.CompanyMunicipality, opt => opt.MapFrom(src => src["company_municipality"]))
                   .ForMember(d => d.CompanyAddress, opt => opt.MapFrom(src => src["company_address"]));
            }).CreateMapper();

            var cashierWithDetails = DataContext.ExecuteSql<CashierWithDetails>(sql, mapper);
            return cashierWithDetails;
        }
    }
}
