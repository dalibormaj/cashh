using Victory.Network.Infrastructure.Repositories;
using Victory.Network.Infrastructure.Repositories.Abstraction;
using Victory.DataAccess;

namespace Victory.Network.Infrastructure.UnitOfWork
{
    public class TestUnitOfWork : BaseUnitOfWork, ITestUnitOfWork
    {
        public ITestRepository TestRepository { get; }

        public TestUnitOfWork(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
        {
            TestRepository = GetRepository<TestRepository>();
        }
    }
}
