using Amazon.Runtime.Internal.Util;
using AppCore.Services;
using AppTests.Entities;
using AppTests.Repositories;
using EventBus;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppTests.Services
{
    public class BaseServiceTests
    {
        private readonly Mock<ITestEntityRepository> _repository;
        private readonly Mock<IEventPublisher> _eventPublisher;
        private readonly Mock<ILogger<BaseService<ITestEntityRepository, TestEntity, Guid>>> _logger;
        public BaseServiceTests()
        {
            _repository = new Mock<ITestEntityRepository>();
            _eventPublisher = new Mock<IEventPublisher>();
            _logger = new Mock<ILogger<BaseService<ITestEntityRepository, TestEntity, Guid>>>();
        }
        [Fact]
        public async Task ChangeAsync()
        {
            TestEntity entity = new TestEntity();
            _repository.Setup(m => m.GetAsync(It.Is<Guid>(id => id == entity.Id))).ReturnsAsync(entity);
            _repository.Setup(m => m.UpdateAsync(It.Is<TestEntity>(te => te.Id == entity.Id))).ReturnsAsync(entity);
            BaseService<ITestEntityRepository, TestEntity, Guid> service
                = new BaseService<ITestEntityRepository, TestEntity, Guid>(_repository.Object, _eventPublisher.Object, _logger.Object);
            bool changed = false;
            Func<TestEntity, TestEntity> changeFunc = (entity) =>
            {
                changed = true;
                return entity;
            };
            TestEntity changeTestEntity = await service.ChangeAsync(entity.Id, changeFunc);
            Assert.True(changed);
            Assert.Equal(entity.Id, changeTestEntity.Id);
        }
    }
}
