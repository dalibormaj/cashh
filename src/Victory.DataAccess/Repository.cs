namespace Victory.DataAccess
{
    public abstract class Repository
    {
        public Repository(IDataContext dataContext, ICacheContext cacheContext)
        {
            DataContext = dataContext;
            CacheContext = cacheContext;
        }

        public IDataContext DataContext { get; }
        public ICacheContext CacheContext { get; }
    }
}