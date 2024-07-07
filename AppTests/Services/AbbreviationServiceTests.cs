using App.Commands;
using App.Entities;
using App.Repositories;
using App.Services;
using EventBus;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppTests.Services
{
    public class AbbreviationServiceTests
    {
        private readonly AbbreviationService _service;
        private readonly Mock<IAbbreviationRepository> _repository;
        private readonly Mock<ILogger<IAbbreviationService>> _logger;
        private readonly Mock<IEventPublisher> _eventPublisher;
        public AbbreviationServiceTests() 
        {
            _repository = new Mock<IAbbreviationRepository>();
            _logger = new Mock<ILogger<IAbbreviationService>>();
            _eventPublisher = new Mock<IEventPublisher>();
            _service = new AbbreviationService(_repository.Object, _logger.Object, _eventPublisher.Object);
        }
        [Fact]
        public async Task Create()
        {
            string shortForm = Guid.NewGuid().ToString();
            Abbreviation abbreviation = await _service.CreateAsync(new CreateAbbreviationCmd(shortForm));
            Assert.Equal(shortForm, abbreviation.ShortForm);
            _repository.Verify(e => e.InsertAsync(It.Is<Abbreviation>(a => a.Id == shortForm)), Times.Once);
        }
        [Fact]
        public async Task Get()
        {
            string name = Guid.NewGuid().ToString();
            Abbreviation abbreviation = new Abbreviation(name);
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == name))).ReturnsAsync(abbreviation);
            Abbreviation dbAbbreviation = await _service.GetAsync(name);
            Assert.Equal(abbreviation, dbAbbreviation);
        }
        [Fact]
        public async Task GetAll()
        {
            List<Abbreviation> abbreviations = new List<Abbreviation>()
            {
                new Abbreviation(Guid.NewGuid().ToString()),
                new Abbreviation(Guid.NewGuid().ToString()),
                new Abbreviation(Guid.NewGuid().ToString())
            };
            _repository.Setup(x => x.GetAsync()).ReturnsAsync(abbreviations);
            List<Abbreviation> dbAbbreviations = (await _service.GetAsync()).ToList();
            Assert.Equal(dbAbbreviations, abbreviations);
        }
        [Fact]
        public async Task Delete()
        {
            string shortForm = Guid.NewGuid().ToString();
            Abbreviation abbreviation = new Abbreviation(shortForm);
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == shortForm))).ReturnsAsync(abbreviation);
            await _service.DeleteAsync(shortForm);
            _repository.Verify(x => x.DeleteAsync(It.Is<Abbreviation>(a => a.ShortForm == shortForm)), Times.Once());
        }
        [Fact]
        public async Task Change()
        {
            string shortForm = Guid.NewGuid().ToString();
            string longForm = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();
            Abbreviation abbreviation = new Abbreviation(shortForm);
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == shortForm))).ReturnsAsync(abbreviation);
            _repository.Setup(x => x.UpdateAsync(It.Is<Abbreviation>(a => a.ShortForm == shortForm))).ReturnsAsync(abbreviation);
            Abbreviation dbAbbreviation = await _service.ChangeAsync(shortForm, new ChangeAbbreviationCmd(
                longForm,
                newDescription));
            Assert.Equal(longForm, dbAbbreviation.LongForm);
            Assert.Equal(newDescription, dbAbbreviation.Description);
            _repository.Verify(x => x.UpdateAsync(It.Is<Abbreviation>(a => a.ShortForm == shortForm)), Times.Once());
        }
    }
}
