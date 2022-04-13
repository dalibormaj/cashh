using System.Threading.Tasks;

namespace Victory.Network.Infrastructure.Repositories.Abstraction
{
    public interface ITestRepository
    {
        Task Exec(int userId, string propertyName, string propertyValue);
    }
}
