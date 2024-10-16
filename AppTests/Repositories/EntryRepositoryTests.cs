using App.Entities;
using App.Repositories;
using App.Repositories.Dtos;
using Db;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppTests.Repositories
{
    public class EntryRepositoryTests
    {
        private readonly EntryRepository _repository;
        private readonly Mock<IDbClient> _dbClient;
        private readonly Mock<ILogger<IEntryRepository>> _logger;
        public EntryRepositoryTests() 
        { 
            _dbClient = new Mock<IDbClient>();
            _logger = new Mock<ILogger<IEntryRepository>>();
            _repository = new EntryRepository(_dbClient.Object, _logger.Object);
        }
        [Fact]
        public async Task New()
        {
            Entry abbreviation = new Entry("CD", Array.Empty<string>());
            _dbClient.Setup(e => e.GetAsync<EntryDto, string>(It.Is<string>(id => id == abbreviation.Name))).ReturnsAsync(abbreviation.GetDto());
            Entry dbAbbreviation = await _repository.GetAsync(abbreviation.Id);
            Assert.NotNull(dbAbbreviation);
            Assert.Equal(abbreviation.GetDto(), dbAbbreviation.GetDto());
        }
    }
}
