using App.Entities;
using App.Repositories;
using Db;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppTests.Repositories
{
    public class AbbreviationRepositoryTests
    {
        private readonly AbbreviationRepository _repository;
        private readonly Mock<IDbClient> _dbClient;
        private readonly Mock<ILogger<IAbbreviationRepository>> _logger;
        public AbbreviationRepositoryTests() 
        { 
            _dbClient = new Mock<IDbClient>();
            _logger = new Mock<ILogger<IAbbreviationRepository>>();
            _repository = new AbbreviationRepository(_dbClient.Object, _logger.Object);
        }
        [Fact]
        public async Task New()
        {
            Abbreviation abbreviation = new Abbreviation("CD");
            _dbClient.Setup(e => e.GetAsync<Abbreviation, string>(It.Is<string>(id => id == abbreviation.ShortForm))).ReturnsAsync(abbreviation);
            Abbreviation dbAbbreviation = await _repository.GetAsync(abbreviation.Id);
            Assert.NotNull(dbAbbreviation);
            Assert.Equal(abbreviation, dbAbbreviation);
        }
    }
}
