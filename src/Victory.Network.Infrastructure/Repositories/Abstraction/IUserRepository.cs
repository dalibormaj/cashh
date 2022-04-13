using System.Collections.Generic;
using System.Threading.Tasks;
using Victory.Network.Infrastructure.Repositories.Models;

namespace Victory.Network.Infrastructure.Repositories.Abstraction
{
    public interface IUserRepository
    {
        Task<User> GetUser(int userId);
        Task<UserDetail> GetUserDetails(int userId);
        Task<List<UserExtraDetail>> GetUserExtraDetails(int userId);
        Task SaveUserDetailExtraProperty(int userId, string propertyName, string propertyValue);
    }
}
