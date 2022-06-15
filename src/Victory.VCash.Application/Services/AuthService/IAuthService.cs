using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Application.Services.AuthService.Results;
using Victory.VCash.Application.Services.CashierService;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Application.Services.AuthService
{
    public interface IAuthService
    {
        Device RegisterDevice(Guid agentId, string deviceName);
        CreateDeviceTokenResult CreateDeviceToken(Guid agentId, string deviceCode);
        ValidateDeviceTokenResult ValidateDeviceToken(string token);
        CreateCashierTokenResult CreateCashierToken(string deviceToken, string userName, string pin);
    }
}
