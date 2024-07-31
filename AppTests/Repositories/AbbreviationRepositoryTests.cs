using App.Entities;
using App.Repositories;
using App.Repositories.Dtos;
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
            Abbreviation abbreviation = new Abbreviation("CD", string.Empty, string.Empty);
            _dbClient.Setup(e => e.GetAsync<AbbreviationDto, string>(It.Is<string>(id => id == abbreviation.ShortForm))).ReturnsAsync(abbreviation.GetDto());
            Abbreviation dbAbbreviation = await _repository.GetAsync(abbreviation.Id);
            Assert.NotNull(dbAbbreviation);
            Assert.Equal(abbreviation.GetDto(), dbAbbreviation.GetDto());
        }
    }
}
