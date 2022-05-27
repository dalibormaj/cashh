using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Application.Services.CashierService
{
    public class AccessTokenValidationResult
    {
        public bool IsValid { get; set; }
        public List<Claim> Claims { get; set; }
        public string ErrorMessage { get; set; }
    }
}
