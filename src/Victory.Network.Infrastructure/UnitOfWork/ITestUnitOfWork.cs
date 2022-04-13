using Victory.Network.Infrastructure.Repositories.Abstraction;
using Victory.DataAccess;

namespace Victory.Network.Infrastructure.UnitOfWork
{
    public interface ITestUnitOfWork : IUnitOfWork
    {
        ITestRepository TestRepository {get;}
    }
}
