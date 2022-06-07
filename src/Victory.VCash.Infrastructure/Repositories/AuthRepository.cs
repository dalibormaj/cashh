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
    public class AuthRepository : Repository, IAuthRepository
    {
        private ICacheContext _cacheContext;

        public AuthRepository(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
        {
            _cacheContext = cacheContext;
        }

        public Device SaveDevice(Device device)
        {
            var s_deviceId = device.DeviceId?.ToString() ?? "null";
            var s_agentId = !string.IsNullOrEmpty(device.AgentId) ? $"'{device.AgentId}'" : "null";
            var s_enabled = device.Enabled?.ToString() ?? "null";
            var s_name = !string.IsNullOrEmpty(device.Name) ? $"'{device.Name}'" : "null";
            var s_deviceCode = !string.IsNullOrEmpty(device.DeviceCode) ? $"'{device.DeviceCode}'" : "null";
            var s_deviceCodeIssuedAt = device.DeviceCodeIssuedAt != null ? $"'{device.DeviceCodeIssuedAt?.ToString("yyyy-MM-dd HH:mm:ss.fff")}'" : "null";
            var s_device_code_expires_in_min = device.DeviceCodeExpiresInMin?.ToString() ?? "null";
            var s_token = !string.IsNullOrEmpty(device.Token) ? $"'{device.Token}'" : "null";
            var s_tokenIssuedAt = device.TokenIssuedAt != null ? $"'{device.TokenIssuedAt?.ToString("yyyy-MM-dd HH:mm:ss.fff")}'" : "null";
            var s_tokenExpiresAt = device.TokenExpiresAt != null ? $"'{device.TokenExpiresAt?.ToString("yyyy-MM-dd HH:mm:ss.fff")}'" : "null";

            var sql = $@"DO $$
                         DECLARE
                             _device_id INT := {s_deviceId};
                             _agent_id UUID := {s_agentId};
                             _enabled BOOLEAN := {s_enabled};
                             _name VARCHAR(200) := {s_name}; 
                             _device_code VARCHAR(200) := {s_deviceCode}; 
                             _device_code_issued_at TIMESTAMP := {s_deviceCodeIssuedAt};
                             _device_code_expires_in_min INT := {s_device_code_expires_in_min};
                             _token VARCHAR(1000) := {s_token};
                             _token_issued_at TIMESTAMP := {s_tokenIssuedAt};
                             _token_expires_at TIMESTAMP := {s_tokenExpiresAt};
                         BEGIN 
                             IF EXISTS(SELECT 'x' FROM device WHERE device_id = _device_id) THEN
	                            UPDATE device SET agent_id = _agent_id,
					                              enabled = _enabled, 
					                              name = _name, 
					                              device_code = _device_code,
                                                  device_code_issued_at = _device_code_issued_at,
                                                  device_code_expires_in_min = _device_code_expires_in_min,
                                                  token = _token,
                                                  token_issued_at = _token_issued_at,
                                                  token_expires_at = _token_expires_at
	                            WHERE device_id = _device_id;
                             ELSE
	                            INSERT INTO device(agent_id, enabled, name, device_code, device_code_issued_at, device_code_expires_in_min, token, token_issued_at, token_expires_at)
	                            VALUES (_agent_id, _enabled, _name, _device_code, _device_code_issued_at, _device_code_expires_in_min, _token, _token_issued_at, _token_expires_at);
                         
	                            _device_id := currval(pg_get_serial_sequence('device','device_id'));
                             END IF;
                         
                             -- create temp table with affected rows
                             CREATE TEMPORARY TABLE _tmp{nameof(Device)} ON COMMIT DROP 
                             AS
                             SELECT * 
                             FROM device 
                             WHERE device_id = _device_id;
                         END $$;
                         
                         -- result
                         SELECT * 
                         FROM _tmp{nameof(Device)}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Device>()
                   .ForMember(d => d.DeviceId, opt => opt.MapFrom(src => src["device_id"]))
                   .ForMember(d => d.AgentId, opt => opt.MapFrom(src => src["agent_id"]))
                   .ForMember(d => d.Enabled, opt => opt.MapFrom(src => src["enabled"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.DeviceCode, opt => opt.MapFrom(src => src["device_code"]))
                   .ForMember(d => d.DeviceCodeIssuedAt, opt => opt.MapFrom(src => src["device_code_issued_at"]))
                   .ForMember(d => d.DeviceCodeExpiresInMin, opt => opt.MapFrom(src => src["device_code_expires_in_min"]))
                   .ForMember(d => d.Token, opt => opt.MapFrom(src => src["token"]))
                   .ForMember(d => d.TokenIssuedAt, opt => opt.MapFrom(src => src["token_issued_at"]))
                   .ForMember(d => d.TokenExpiresAt, opt => opt.MapFrom(src => src["token_expires_at"]));
            }).CreateMapper();

            var result = DataContext.ExecuteSql<Device>(sql, mapper).FirstOrDefault();
            return result;
        }

        public Device GetDevice(int deviceId)
        {
            var sql = $@"SELECT device_id, agent_id, enabled, name, device_code
                         FROM public.device
                         WHERE device_id = {deviceId}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Device>()
                   .ForMember(d => d.DeviceId, opt => opt.MapFrom(src => src["device_id"]))
                   .ForMember(d => d.AgentId, opt => opt.MapFrom(src => src["agent_id"]))
                   .ForMember(d => d.Enabled, opt => opt.MapFrom(src => src["enabled"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.DeviceCode, opt => opt.MapFrom(src => src["device_code"]));
            }).CreateMapper();

            var device = DataContext.ExecuteSql<Device>(sql, mapper).SingleOrDefault();
            return device;
        }

        public List<Device> GetDevices(string agentId, string deviceName = null, string deviceCode = null, string token = null)
        {

            string s_deviceName = deviceName != null ? $"'{deviceName}'" : "null";
            string s_deviceCode = deviceCode != null ? $"'{deviceCode}'" : "null";
            string s_token = token != null ? $"'{token}'" : "null";

            var sql = $@"SELECT device_id, agent_id, enabled, name, device_code, token, token_issued_at, token_expires_at
                         FROM public.device d
                         WHERE agent_id = '{agentId}' 
                           AND UPPER(COALESCE(name, '')) = UPPER(COALESCE({s_deviceName}, d.name, ''))
                           AND UPPER(COALESCE(device_code, '')) = UPPER(COALESCE({s_deviceCode}, d.device_code, ''))
                           AND UPPER(COALESCE(token, '')) = UPPER(COALESCE({s_token}, d.token, ''))";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Device>()
                   .ForMember(d => d.DeviceId, opt => opt.MapFrom(src => src["device_id"]))
                   .ForMember(d => d.AgentId, opt => opt.MapFrom(src => src["agent_id"]))
                   .ForMember(d => d.Enabled, opt => opt.MapFrom(src => src["enabled"]))
                   .ForMember(d => d.Name, opt => opt.MapFrom(src => src["name"]))
                   .ForMember(d => d.DeviceCode, opt => opt.MapFrom(src => src["device_code"]))
                   .ForMember(d => d.Token, opt => opt.MapFrom(src => src["token"]))
                   .ForMember(d => d.TokenIssuedAt, opt => opt.MapFrom(src => src["token_issued_at"]))
                   .ForMember(d => d.TokenExpiresAt, opt => opt.MapFrom(src => src["token_expires_at"]));
            }).CreateMapper();

            var devices = DataContext.ExecuteSql<Device>(sql, mapper);
            return devices;
        }
    }
}
