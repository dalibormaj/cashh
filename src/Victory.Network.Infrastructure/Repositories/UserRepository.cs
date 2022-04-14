using AutoMapper;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.Network.Domain.Models;
using Victory.Network.Infrastructure.Repositories.Abstraction;
using Victory.Network.Infrastructure.Repositories.Enums;

namespace Victory.Network.Infrastructure.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        private ICacheContext _cacheContext;

        public UserRepository(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
        {
            _cacheContext = cacheContext;
        }

        public async Task SaveUser(User user)
        {
            var sql = $@"WITH tmp (user_id, parent_id) AS (
                            VALUES ({user.UserId},{user.ParentId})
                         )

                         INSERT INTO victory_network.user(user_id, parent_id)
                         SELECT t.user_id, t.parent_id FROM tmp t
                             ON CONFLICT(user_id) DO
                                UPDATE SET parent_id = EXCLUDED.parent_id;";
            await DataContext.ExecuteSqlAsync(sql);
        }

        public async Task<User> GetUser(int userId)
        {
            var sql = $@"SELECT user_id, parent_id, name, last_name, user_status_id
                         FROM victory_network.user
                         WHERE user_id = {userId}";
            
            var user = (await DataContext.ExecuteSqlAsync<User>(sql)).SingleOrDefault();
            return user;
        }
    }
}


