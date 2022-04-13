using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.Network.Infrastructure.Repositories.Abstraction;
using Victory.Network.Infrastructure.Repositories.Enums;

namespace Victory.Network.Infrastructure.Repositories
{
    public class TestRepository : Repository, ITestRepository
	{
		private Brand _brand = Brand.Victory247SRB;

		public TestRepository(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
		{
		}

		public async Task Exec(int userId, string propertyName, string propertyValue)
        {
			var sql = $"Select Broj from TestTable";
			var a = await DataContext.ExecuteSqlAsync(sql);

		}
    }
}


