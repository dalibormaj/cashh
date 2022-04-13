using System;
using System.Data;

namespace Victory.DataAccess
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        protected IDataContext _dataContext;
        protected ICacheContext _cacheContext;

        public BaseUnitOfWork(IDataContext dataContext, ICacheContext cacheContext)
        {
            if(dataContext == null)
                throw new InvalidOperationException("DataContext is missing");

            _dataContext = dataContext;
            _cacheContext = cacheContext;
        }

        public IDbTransaction BeginTransaction()
        {
            if (_dataContext.Connection == null) 
                throw new InvalidOperationException("Transaction cannot start on empty connection. Check DataContext!");
            return _dataContext.BeginTransaction();
        }

        public void Commit()
        {
            _dataContext.Commit();
        }

        public void Rollback()
        {
            _dataContext.Rollback();
        }

        //Note: it's important for repositories to share the same DataContext
        public virtual T GetRepository<T>() where T : Repository
        {
            return (T)Activator.CreateInstance(typeof(T), _dataContext, _cacheContext);
        }
    }
}
