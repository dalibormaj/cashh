using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Application.Services.AuthService.Results
{
    public class ValidateCashierTokenResult
    {
        public bool IsValid { get; set; }
        public List<Claim> Claims { get; set; }
        public List<Error> Errors { get; set; }
    }
}
