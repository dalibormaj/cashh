using Victory.DataAccess;
using Victory.Network.Infrastructure.Repositories.Abstraction;

namespace Victory.Network.Infrastructure.UnitOfWork
{
    public interface ITpsUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}
