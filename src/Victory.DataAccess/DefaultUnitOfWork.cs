namespace Victory.DataAccess
{
    public class DefaultUnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public DefaultUnitOfWork(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
        {
        }
    }
}
