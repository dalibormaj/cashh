using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Infrastructure.Repositories.Abstraction
{
    public interface IAuthRepository
    {
        Device GetDevice(int deviceId);
        List<Device> GetDevices(string agentId, string deviceName = null, string deviceCode = null, string token = null);
        Device SaveDevice(Device device);
    }
}
