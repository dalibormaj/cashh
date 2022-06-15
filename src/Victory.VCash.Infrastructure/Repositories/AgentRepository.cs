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

        public Agent GetAgent(Guid agentId)
        {
            var sql = $@"SELECT *
                         FROM public.agent
                         WHERE agent_id = '{agentId}'";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Agent>()
                   .ForMember(d => d.AgentId, opt => opt.MapFrom(src => src["agent_id"]))
                   .ForMember(d => d.AgentStatusId, opt => opt.MapFrom(src => src["agent_status_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src["first_name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
                   .ForMember(d => d.Email, opt => opt.MapFrom(src => src["email"]))
                   .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(src => src["phone_number"]))
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.RefferalCode, opt => opt.MapFrom(src => src["refferal_code"]))
                   .ForMember(d => d.Error, opt => opt.MapFrom(src => src["error"]));
            }).CreateMapper();

            var agent = DataContext.ExecuteSql<Agent>(sql, mapper).SingleOrDefault();
            return agent;
        }

        public List<Agent> GetAgents(Guid? agentId = null, int? userId = null, string userName = null, string email = null, int? companyId = null)
        {
            string s_agentId = agentId != null? $"'{agentId}'" : "null";
            string s_userId = userId?.ToString() ?? "null";
            string s_userName = userName != null ? $"'{userName}'" : "null";
            string s_email = email != null ? $"'{email}'" : "null";
            string s_companyId = companyId?.ToString() ?? "null";

            var sql = $@"SELECT *
                         FROM public.agent a
                         WHERE a.agent_id = COALESCE({s_agentId}, a.agent_id)
                           AND COALESCE(a.user_id, 0) = COALESCE({s_userId}, a.user_id, 0)
                           AND COALESCE(a.user_name, '') = COALESCE({s_userName}, a.user_name, '')
                           AND COALESCE(a.email, '') = COALESCE({s_email}, a.email, '')
                           AND COALESCE(a.company_id, 0) = COALESCE({s_companyId}, a.company_id, 0)";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Agent>()
                   .ForMember(d => d.AgentId, opt => opt.MapFrom(src => src["agent_id"]))
                   .ForMember(d => d.AgentStatusId, opt => opt.MapFrom(src => src["agent_status_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src["first_name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
                   .ForMember(d => d.Email, opt => opt.MapFrom(src => src["email"]))
                   .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(src => src["phone_number"]))
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.RefferalCode, opt => opt.MapFrom(src => src["refferal_code"]))
                   .ForMember(d => d.Error, opt => opt.MapFrom(src => src["error"]));
            }).CreateMapper();

            var agents = DataContext.ExecuteSql<Agent>(sql, mapper);
            return agents;
        }

        public Company GetCompany(int companyId)
        {
            var sql = $@"SELECT *
                         FROM public.company
                         WHERE company_id = {companyId}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Company>()
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.RegistrationNumber, opt => opt.MapFrom(src => src["registration_number"]))
                   .ForMember(d => d.TaxNumber, opt => opt.MapFrom(src => src["tax_number"]))
                   .ForMember(d => d.City, opt => opt.MapFrom(src => src["city"]))
                   .ForMember(d => d.Municipality, opt => opt.MapFrom(src => src["municipality"]))
                   .ForMember(d => d.Address, opt => opt.MapFrom(src => src["address"]))
                   .ForMember(d => d.GooglePlaceId, opt => opt.MapFrom(src => src["google_place_id"]))
                   .ForMember(d => d.GoogleFullAddress, opt => opt.MapFrom(src => src["google_full_address"]))
                   .ForMember(d => d.Latitude, opt => opt.MapFrom(src => src["latitude"]))
                   .ForMember(d => d.Longitude, opt => opt.MapFrom(src => src["longitude"]));
            }).CreateMapper();

            var company = DataContext.ExecuteSql<Company>(sql, mapper).SingleOrDefault();
            return company;
        }

        public List<Company> GetCompanies(int? companyId = null, string taxNumber = null)
        {
            string s_company_id = companyId != null ? $"'{companyId}'" : "null";
            string s_taxNumber = !string.IsNullOrEmpty(taxNumber) ? $"'{taxNumber}'" : "null";

            var sql = $@"SELECT company_id, name, registration_number, tax_number
                         FROM public.company c
                         WHERE c.company_id = COALESCE({s_company_id}, c.company_id)
                           AND COALESCE(c.tax_number, '') = COALESCE({s_taxNumber}, c.tax_number, '')";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Company>()
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.RegistrationNumber, opt => opt.MapFrom(src => src["registration_number"]))
                   .ForMember(d => d.TaxNumber, opt => opt.MapFrom(src => src["tax_number"]))
                   .ForMember(d => d.City, opt => opt.MapFrom(src => src["city"]))
                   .ForMember(d => d.Municipality, opt => opt.MapFrom(src => src["municipality"]))
                   .ForMember(d => d.Address, opt => opt.MapFrom(src => src["address"]))
                   .ForMember(d => d.GooglePlaceId, opt => opt.MapFrom(src => src["google_place_id"]))
                   .ForMember(d => d.GoogleFullAddress, opt => opt.MapFrom(src => src["google_full_address"]))
                   .ForMember(d => d.Latitude, opt => opt.MapFrom(src => src["latitude"]))
                   .ForMember(d => d.Longitude, opt => opt.MapFrom(src => src["longitude"]));
            }).CreateMapper();

            var companies = DataContext.ExecuteSql<Company>(sql, mapper);
            return companies;
        }

        public Venue GetVenue(int venueId)
        {
            var sql = $@"SELECT *
                         FROM public.venue
                         WHERE venue_id = {venueId}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Venue>()
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.City, opt => opt.MapFrom(src => src["city"]))
                   .ForMember(d => d.Municipality, opt => opt.MapFrom(src => src["municipality"]))
                   .ForMember(d => d.Address, opt => opt.MapFrom(src => src["address"]))
                   .ForMember(d => d.GooglePlaceId, opt => opt.MapFrom(src => src["google_place_id"]))
                   .ForMember(d => d.GoogleFullAddress, opt => opt.MapFrom(src => src["google_full_address"]))
                   .ForMember(d => d.Latitude, opt => opt.MapFrom(src => src["latitude"]))
                   .ForMember(d => d.Longitude, opt => opt.MapFrom(src => src["longitude"]));
            }).CreateMapper();

            var venue = DataContext.ExecuteSql<Venue>(sql, mapper).SingleOrDefault();
            return venue;
        }

        public List<Venue> GetVenues(int? companyId = null)
        {
            string s_company_id = companyId != null ? $"'{companyId}'" : "null";

            var sql = $@"SELECT venue_id, company_id, name, city, municipality, address
                         FROM public.venue v
                         WHERE v.company_id = COALESCE({s_company_id}, v.company_id)";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Venue>()
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.City, opt => opt.MapFrom(src => src["city"]))
                   .ForMember(d => d.Municipality, opt => opt.MapFrom(src => src["municipality"]))
                   .ForMember(d => d.Address, opt => opt.MapFrom(src => src["address"]))
                   .ForMember(d => d.GooglePlaceId, opt => opt.MapFrom(src => src["google_place_id"]))
                   .ForMember(d => d.GoogleFullAddress, opt => opt.MapFrom(src => src["google_full_address"]))
                   .ForMember(d => d.Latitude, opt => opt.MapFrom(src => src["latitude"]))
                   .ForMember(d => d.Longitude, opt => opt.MapFrom(src => src["longitude"]));
            }).CreateMapper();

            var venues = DataContext.ExecuteSql<Venue>(sql, mapper);
            return venues;
        }

        public Agent SaveAgent(Agent agent)
        {
            var s_agent_id = agent.AgentId != null ? $"'{agent.AgentId}'" : $"'{Guid.NewGuid()}'";
            var s_userId = agent.UserId?.ToString() ?? "null";
            var s_agentStatusId = ((int?)agent.AgentStatusId)?.ToString() ?? "null";
            var s_parentAgentId = !string.IsNullOrEmpty(agent.ParentAgentId?.ToString()) ? $"'{agent.ParentAgentId}'" : "null";
            var s_userName = !string.IsNullOrEmpty(agent.UserName) ? $"'{agent.UserName}'" : "null";
            var s_firstName = !string.IsNullOrEmpty(agent.FirstName) ? $"'{agent.FirstName}'" : "null";
            var s_lastName = !string.IsNullOrEmpty(agent.LastName) ? $"'{agent.LastName}'" : "null";
            var s_email = !string.IsNullOrEmpty(agent.Email) ? $"'{agent.Email}'" : "null";
            var s_phoneNumber  = !string.IsNullOrEmpty(agent.PhoneNumber) ? $"'{agent.PhoneNumber}'" : "null";
            var s_companyId = agent.CompanyId?.ToString() ?? "null";
            var s_refferalCode = !string.IsNullOrEmpty(agent.RefferalCode) ? $"'{agent.RefferalCode}'" : "null";
            var s_error = !string.IsNullOrEmpty(agent.Error) ? $"'{agent.Error}'" : "null";
            var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var sql = $@"DO $$
                         DECLARE
	                         _agent_id UUID := {s_agent_id};
                             _agent_status_id INT := {s_agentStatusId};
                             _parent_agent_id UUID := {s_parentAgentId};
                             _user_id INT := {s_userId};
	                         _user_name VARCHAR(100) := {s_userName};
                             _first_name VARCHAR(100) := {s_firstName};
                             _last_name VARCHAR(100) := {s_lastName};
                             _email VARCHAR(200) := {s_email};
                             _phone_number VARCHAR(100) := {s_phoneNumber};
	                         _company_id INT := {s_companyId};
	                         _refferal_code VARCHAR(100) := {s_refferalCode}; 
                             _error TEXT := {s_error};
                             _now TIMESTAMP := NOW() AT time zone 'utc';
                         BEGIN 
	                         IF EXISTS(SELECT 'x' FROM agent WHERE agent_id = _agent_id) THEN
		                        UPDATE agent SET agent_status_id = _agent_status_id,
						                         parent_agent_id = _parent_agent_id,
                                                 user_id = _user_id,
						                         user_name = _user_name,
                                                 first_name = _first_name,
                                                 last_name = _last_name,
                                                 email = _email,
                                                 phone_number = _phone_number,
						                         company_id = _company_id,
                                                 refferal_code = _refferal_code,
                                                 error = _error
		                        WHERE agent_id = _agent_id;
	                         ELSE
                                INSERT INTO public.agent(agent_id, agent_status_id, parent_agent_id, user_id, user_name, first_name, last_name, email, phone_number, company_id, refferal_code, error, insert_date) 
		                        VALUES (_agent_id, _agent_status_id, _parent_agent_id, _user_id, _user_name, _first_name, _last_name, _email, _phone_number, _company_id, _refferal_code, _error, _now);
	                         END IF;

	                         -- create temp table with affected rows
	                         CREATE TEMPORARY TABLE _tmp{unixMs} ON COMMIT DROP 
	                         AS
	                         SELECT * 
	                         FROM agent 
	                         WHERE agent_id = _agent_id;
                         END $$;

                         -- result
                         SELECT * 
                         FROM _tmp{unixMs}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Agent>()
                   .ForMember(d => d.AgentId, opt => opt.MapFrom(src => src["agent_id"]))
                   .ForMember(d => d.AgentStatusId, opt => opt.MapFrom(src => src["agent_status_id"]))
                   .ForMember(d => d.ParentAgentId, opt => opt.MapFrom(src => src["parent_agent_id"]))
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src["first_name"]))
                   .ForMember(d => d.LastName, opt => opt.MapFrom(src => src["last_name"]))
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
            var s_companyId = company.CompanyId?.ToString() ?? "null";
            var s_name = !string.IsNullOrEmpty(company.Name) ? $"'{company.Name}'" : "null";
            var s_registrationNumber = !string.IsNullOrEmpty(company.RegistrationNumber) ? $"'{company.RegistrationNumber}'" : "null";
            var s_taxNumber = !string.IsNullOrEmpty(company.TaxNumber) ? $"'{company.TaxNumber}'" : "null";
            var s_city = !string.IsNullOrEmpty(company.City) ? $"'{company.City}'" : "null";
            var s_municipality = !string.IsNullOrEmpty(company.Municipality) ? $"'{company.Municipality}'" : "null";
            var s_address = !string.IsNullOrEmpty(company.Address) ? $"'{company.Address}'" : "null";
            var s_googlePlaceId = !string.IsNullOrEmpty(company.GooglePlaceId) ? $"'{company.GooglePlaceId}'" : "null";
            var s_googleFullAddress = !string.IsNullOrEmpty(company.GoogleFullAddress) ? $"'{company.GoogleFullAddress}'" : "null";
            var s_latitude = company.Latitude?.ToString() ?? "null";
            var s_longitude = company.Longitude?.ToString() ?? "null";
            var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var sql = $@"DO $$
                         DECLARE
	                         _company_id INT := {s_companyId};
                             _name VARCHAR(300) := {s_name};
                             _registration_number VARCHAR(20) := {s_registrationNumber};
                             _tax_number VARCHAR(20) := {s_taxNumber};
                             _city VARCHAR(100) := {s_city};
                             _municipality VARCHAR(100) := {s_municipality};
                             _address VARCHAR(200) := {s_address};
                             _google_place_id VARCHAR(200) := {s_googlePlaceId};
                             _google_full_address VARCHAR(400) := {s_googleFullAddress};
                             _latitude DECIMAL(19,5) := {s_latitude};
                             _longitude DECIMAL(19,5) := {s_longitude};
                         BEGIN 
	                         IF EXISTS(SELECT 'x' FROM company WHERE company_id = _company_id) THEN
		                        UPDATE company SET name = _name,
						                           registration_number = _registration_number,
						                           tax_number = _tax_number,
                                                   city = _city,
                                                   municipality = _municipality,
                                                   address = _address,
                                                   google_place_id = _google_place_id,
                                                   google_full_address = _google_full_address,
                                                   latitude = _latitude,
                                                   longitude = _longitude
		                        WHERE company_id = _company_id;
	                         ELSE
                                INSERT INTO public.company(name, registration_number, tax_number, city, municipality, address, google_place_id, google_full_address, latitude, longitude) 
		                        VALUES (_name, _registration_number, _tax_number, _city, _municipality, _address, _google_place_id, _google_full_address, _latitude, _longitude);

                                _company_id := currval(pg_get_serial_sequence('company','company_id'));
	                         END IF;

	                         -- create temp table with affected rows
	                         CREATE TEMPORARY TABLE _tmp{unixMs} ON COMMIT DROP 
	                         AS
	                         SELECT * 
	                         FROM company
	                         WHERE company_id = _company_id;
                         END $$;

                         -- result
                         SELECT * 
                         FROM _tmp{unixMs}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Company>()
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.RegistrationNumber, opt => opt.MapFrom(src => src["registration_number"]))
                   .ForMember(d => d.TaxNumber, opt => opt.MapFrom(src => src["tax_number"]))
                   .ForMember(d => d.City, opt => opt.MapFrom(src => src["city"]))
                   .ForMember(d => d.Municipality, opt => opt.MapFrom(src => src["municipality"]))
                   .ForMember(d => d.Address, opt => opt.MapFrom(src => src["address"]))
                   .ForMember(d => d.GooglePlaceId, opt => opt.MapFrom(src => src["google_place_id"]))
                   .ForMember(d => d.GoogleFullAddress, opt => opt.MapFrom(src => src["google_full_address"]))
                   .ForMember(d => d.Latitude, opt => opt.MapFrom(src => src["latitude"]))
                   .ForMember(d => d.Longitude, opt => opt.MapFrom(src => src["longitude"]));
            }).CreateMapper();

            var result = DataContext.ExecuteSql<Company>(sql, mapper).FirstOrDefault();
            return result;
        }

        public Venue SaveVenue(Venue venue)
        {
            var s_venueId = venue.VenueId?.ToString() ?? "null";
            var s_companyId = venue.CompanyId != null ? $"'{venue.CompanyId}'" : "null";
            var s_name = !string.IsNullOrEmpty(venue.Name) ? $"'{venue.Name}'" : "null";
            var s_city = !string.IsNullOrEmpty(venue.City) ? $"'{venue.City}'" : "null";
            var s_municipality = !string.IsNullOrEmpty(venue.Municipality) ? $"'{venue.Municipality}'" : "null";
            var s_address = !string.IsNullOrEmpty(venue.Address) ? $"'{venue.Address}'" : "null";
            var s_googlePlaceId = !string.IsNullOrEmpty(venue.GooglePlaceId) ? $"'{venue.GooglePlaceId}'" : "null";
            var s_googleFullAddress = !string.IsNullOrEmpty(venue.GoogleFullAddress) ? $"'{venue.GoogleFullAddress}'" : "null";
            var s_latitude = venue.Latitude?.ToString() ?? "null";
            var s_longitude = venue.Longitude?.ToString() ?? "null";

            var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var sql = $@"DO $$
                         DECLARE
	                         _venue_id INT := {s_venueId};
                             _company_id INT := {s_companyId};
                             _name VARCHAR(300) := {s_name};
                             _city VARCHAR(100) := {s_city};
                             _municipality VARCHAR(100) := {s_municipality};
                             _address VARCHAR(200) := {s_address};
                             _google_place_id VARCHAR(200) := {s_googlePlaceId};
                             _google_full_address VARCHAR(400) := {s_googleFullAddress};
                             _latitude DECIMAL(19,5) := {s_latitude};
                             _longitude DECIMAL(19,5) := {s_longitude};
                         BEGIN 
	                         IF EXISTS(SELECT 'x' FROM venue WHERE venue_id = _venue_id) THEN
		                        UPDATE venue SET company_id = _company_id,
                                                 name = _name,
                                                 city = _city,
                                                 municipality = _municipality,
                                                 address = _address,
                                                 google_place_id = _google_place_id,
                                                 google_full_address = _google_full_address,
                                                 latitude = _latitude,
                                                 longitude = _longitude
		                        WHERE venue_id = _venue_id;
	                         ELSE
                                INSERT INTO public.venue(company_id, name, city, municipality, address, google_place_id, google_full_address, latitude, longitude) 
		                        VALUES (_company_id, _name, _city, _municipality, _address, _google_place_id, _google_full_address, _latitude, _longitude);

                                _company_id := currval(pg_get_serial_sequence('venue','venue_id'));
	                         END IF;

	                         -- create temp table with affected rows
	                         CREATE TEMPORARY TABLE _tmp{unixMs} ON COMMIT DROP 
	                         AS
	                         SELECT * 
	                         FROM venue
	                         WHERE venue_id = _venue_id;
                         END $$;

                         -- result
                         SELECT * 
                         FROM _tmp{unixMs}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Venue>()
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.City, opt => opt.MapFrom(src => src["city"]))
                   .ForMember(d => d.Municipality, opt => opt.MapFrom(src => src["municipality"]))
                   .ForMember(d => d.Address, opt => opt.MapFrom(src => src["address"]))
                   .ForMember(d => d.GooglePlaceId, opt => opt.MapFrom(src => src["google_place_id"]))
                   .ForMember(d => d.GoogleFullAddress, opt => opt.MapFrom(src => src["google_full_address"]))
                   .ForMember(d => d.Latitude, opt => opt.MapFrom(src => src["latitude"]))
                   .ForMember(d => d.Longitude, opt => opt.MapFrom(src => src["longitude"]));
            }).CreateMapper();

            var result = DataContext.ExecuteSql<Venue>(sql, mapper).FirstOrDefault();
            return result;
        }

        public BankAccount GetBankAccount(int bankAccountId)
        {
            var sql = $@"SELECT *
                         FROM public.bank_account
                         WHERE company_id = '{bankAccountId}'";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, BankAccount>()
                   .ForMember(d => d.BankAccountId, opt => opt.MapFrom(src => src["bank_account_id"]))
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.Bank, opt => opt.MapFrom(src => src["bank"]))
                   .ForMember(d => d.AccountNumber, opt => opt.MapFrom(src => src["account_number"]));
            }).CreateMapper();

            var bankAccount = DataContext.ExecuteSql<BankAccount>(sql, mapper).SingleOrDefault();
            return bankAccount;
        }

        public List<BankAccount> GetBankAccounts(int? bankAccountId = null, int? companyId = null)
        {
            string s_bankAccountId = bankAccountId != null ? $"'{bankAccountId}'" : "null";
            string s_company_id = companyId != null ? $"'{companyId}'" : "null";

            var sql = $@"SELECT bank_account_id, company_id, bank, account_number
                         FROM public.bank_account b
                         WHERE b.bank_account_id = COALESCE({s_bankAccountId}, b.bank_account_id)
                           AND b.company_id = COALESCE({ s_company_id}, b.company_id)";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, BankAccount>()
                   .ForMember(d => d.BankAccountId, opt => opt.MapFrom(src => src["bank_account_id"]))
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.Bank, opt => opt.MapFrom(src => src["bank"]))
                   .ForMember(d => d.AccountNumber, opt => opt.MapFrom(src => src["account_number"]));
            }).CreateMapper();

            var bankAccounts = DataContext.ExecuteSql<BankAccount>(sql, mapper);
            return bankAccounts;
        }

        public BankAccount SaveBankAccount(BankAccount bankAccount)
        {
            var s_bankAccountId = bankAccount.BankAccountId?.ToString() ?? "null";
            var s_companyId = bankAccount.CompanyId != null ? $"'{bankAccount.CompanyId}'" : "null";
            var s_bank = !string.IsNullOrEmpty(bankAccount.Bank) ? $"'{bankAccount.Bank}'" : "null";
            var s_account_number = !string.IsNullOrEmpty(bankAccount.AccountNumber) ? $"'{bankAccount.AccountNumber}'" : "null";
            var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var sql = $@"DO $$
                         DECLARE
	                         _bank_account_id INT := {s_bankAccountId};
                             _company_id INT := {s_companyId};
                             _bank VARCHAR(300) := {s_bank};
                             _account_number VARCHAR(100) := {s_account_number};
                         BEGIN 
	                         IF EXISTS(SELECT 'x' FROM bank_account WHERE bank_account_id = _bank_account_id) THEN
		                        UPDATE bank_account SET company_id = _company_id,
                                                        bank = _bank,
                                                        account_number = _account_number
		                        WHERE bank_account_id = _bank_account_id;
	                         ELSE
                                INSERT INTO public.bank_account(company_id, bank, account_number) 
		                        VALUES (_company_id, _bank, _account_number);

                                _company_id := currval(pg_get_serial_sequence('bank_account','bank_account_id'));
	                         END IF;

	                         -- create temp table with affected rows
	                         CREATE TEMPORARY TABLE _tmp{unixMs} ON COMMIT DROP 
	                         AS
	                         SELECT * 
	                         FROM bank_account
	                         WHERE bank_account_id = _bank_account_id;
                         END $$;

                         -- result
                         SELECT * 
                         FROM _tmp{unixMs}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, BankAccount>()
                   .ForMember(d => d.BankAccountId, opt => opt.MapFrom(src => src["bank_account_id"]))
                   .ForMember(d => d.CompanyId, opt => opt.MapFrom(src => src["company_id"]))
                   .ForMember(d => d.Bank, opt => opt.MapFrom(src => src["bank"]))
                   .ForMember(d => d.AccountNumber, opt => opt.MapFrom(src => src["account_number"]));
            }).CreateMapper();

            var result = DataContext.ExecuteSql<BankAccount>(sql, mapper).FirstOrDefault();
            return result;
        }
    }
}
