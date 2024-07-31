using AppCore.Repositories;
using AppTests.Entities;
using Db;
using Microsoft.Extensions.Logging;

namespace AppTests.Repositories
{
    public interface ITestEntityRepository : IBaseRepository<TestEntity, TestEntityDto, Guid>
    {
    }
    public class TestEntityRepository : BaseRepository<ITestEntityRepository, TestEntity, TestEntityDto, Guid>
    {
        public TestEntityRepository(IDbClient client, ILogger<ITestEntityRepository> logger) : base(client,logger)
        {
        }
    }
}
