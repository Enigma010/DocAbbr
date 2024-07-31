using App.Entities;
using App.Repositories;
using App.Repositories.Dtos;
using AppEvents;
using Db;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace AppTests.Repositories
{
    public class ConfigRepositoryTests
    {
        private readonly Mock<ILogger<IConfigRepository>> _logger;
        private readonly Mock<IDbClient> _client;
        private ConfigRepository _repository;
        public ConfigRepositoryTests()
        {
            _logger = new Mock<ILogger<IConfigRepository>>();
            _client = new Mock<IDbClient>();
            _repository = new ConfigRepository(_client.Object, _logger.Object);
        }
        [Fact]
        public async Task ChangeAsync()
        {
            Config config = new Config();
            Config changeConfig = await _repository.InsertAsync(config);
            Assert.Equal(config.Id, changeConfig.Id);
            _client.Verify(m => m.InsertAsync<ConfigDto, Guid>(config.GetDto()), Times.Once());
        }
        [Fact]
        public async Task GetAsync()
        {
            Config config = new Config();
            _client.Setup(m => m.GetAsync<ConfigDto, Guid>(It.Is<Guid>(id => id == config.Id))).ReturnsAsync(config.GetDto());
            Config changeConfig = await _repository.InsertAsync(config);
            Assert.Equal(config.Id, changeConfig.Id);
            Config getConfig = await _repository.GetAsync(config.Id);
            Assert.Equal(config.Id, getConfig.Id);
            Assert.Empty(getConfig.GetEvents());
            _client.Verify(m => m.GetAsync<ConfigDto, Guid>(config.Id), Times.Once);
        }
        [Fact]
        public async Task GetExpressionAsync()
        {
            List<Config> configs = new List<Config>()
            {
                new Config(),
                new Config(),
                new Config()
            };
            List<ConfigDto> configDtos = configs.Select(config => config.GetDto()).ToList();
            Expression<Func<ConfigDto, bool>> filter = (c) => c.Enabled == true;
            _client.Setup(m => m.GetAsync(It.IsAny<Expression<Func<ConfigDto, bool>>>())).ReturnsAsync(configDtos);
            IEnumerable<Config> getConfigs = await _repository.GetAsync(filter);
            Assert.Equal(configs.Select(c => c.GetDto()), getConfigs.Select(c => c.GetDto()));
            getConfigs.ToList().ForEach(getConfig =>
            {
                Assert.Empty(getConfig.GetEvents());
            });
        }
        [Fact]
        public async Task GetAllAsync()
        {
            List<Config> configs = new List<Config>()
            {
                new Config(),
                new Config(),
                new Config()
            };
            _client.Setup(m => m.GetAsync<ConfigDto, Guid>()).ReturnsAsync(configs.Select(c => c.GetDto()));
            IEnumerable<Config> getConfigs = await _repository.GetAsync();
            Assert.Equal(configs.Count, getConfigs.Count());
            _client.Verify(m => m.GetAsync<ConfigDto, Guid>(), Times.Once);
            getConfigs.ToList().ForEach(getConfig =>
            {
                Assert.Empty(getConfig.GetEvents());
            });
        }
        [Fact]
        public async Task UpdateAsync()
        {
            Config config = new Config();
            Config updateConfig = await _repository.UpdateAsync(config);
            Assert.Equal(config.Id, updateConfig.Id);
            _client.Verify(m => m.UpdateAsync<ConfigDto, Guid>(config.GetDto()), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync()
        {
            ConfigRepository repository = new ConfigRepository(_client.Object, _logger.Object);
            Config config = new Config();
            config.ClearEvents();
            await repository.DeleteAsync(config);
            _client.Verify(m => m.DeleteAsync<ConfigDto, Guid>(It.Is<ConfigDto>(c => c.Id == config.Id)), Times.Once);
            Assert.Collection(
                config.GetEvents(), 
                (c) =>
                {
                    Assert.IsType<ConfigDeletedEvent>(c);
                });
        }
    }
}
