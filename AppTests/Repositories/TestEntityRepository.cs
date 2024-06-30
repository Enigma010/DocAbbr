using AppCore.Repositories;
using AppTests.Entities;
using Db;
using Microsoft.Extensions.Logging;

namespace AppTests.Repositories
{
    public interface ITestEntityRepository : IBaseRepository<TestEntity, Guid>
    {
    }
    public class TestEntityRepository : BaseRepository<ITestEntityRepository, TestEntity, Guid>
    {
        public TestEntityRepository(IDbClient client, ILogger<ITestEntityRepository> logger) : base(client,logger)
        {
        }
    }
}
