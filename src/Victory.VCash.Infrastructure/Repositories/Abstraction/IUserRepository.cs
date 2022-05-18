using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;

namespace Victory.VCash.Infrastructure.Repositories.Abstraction
{
    public interface IUserRepository
    {
        User GetUser(int userId);
        User GetUserByAffiliateCode(string affiliateCode);
        void SaveUser(User user);
    }
}
