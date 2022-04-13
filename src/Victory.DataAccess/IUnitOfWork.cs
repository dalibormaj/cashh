using System.Data;

namespace Victory.DataAccess
{
    public interface IUnitOfWork
    {
        IDbTransaction BeginTransaction();
        void Commit();
        void Rollback();
        T GetRepository<T>() where T : Repository;
    }
}
