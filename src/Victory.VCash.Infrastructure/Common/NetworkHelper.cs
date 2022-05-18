using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.Common
{
    public static class NetworkHelper
    {
        public static string GetLocalIp()
        {
            var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip?.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            return ip ?? "127.0.0.1";
        }
    }
}
