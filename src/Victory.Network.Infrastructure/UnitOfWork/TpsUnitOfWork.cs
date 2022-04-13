using Victory.DataAccess;
using Victory.Network.Infrastructure.Repositories;
using Victory.Network.Infrastructure.Repositories.Abstraction;

namespace Victory.Network.Infrastructure.UnitOfWork
{
    public class TpsUnitOfWork : BaseUnitOfWork, ITpsUnitOfWork
    {
        public IUserRepository UserRepository { get; }

        public TpsUnitOfWork(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
        {
            UserRepository = GetRepository<UserRepository>();
        }
    }
}
