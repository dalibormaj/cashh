using System.Collections.Generic;
using System.Threading.Tasks;
using Victory.Network.Domain.Models;

namespace Victory.Network.Infrastructure.Repositories.Abstraction
{
    public interface IUserRepository
    {
        Task<User> GetUser(int userId);
        Task SaveUser(User user);
    }
}
