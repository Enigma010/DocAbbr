using App.Commands;
using App.Entities;
using App.Repositories;
using App.Services;
using App.Utilities;
using EventBus;
using Markdig;
using Markdown;
using MassTransit.Logging;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace AppTests.Services
{
    public class AbbreviationServiceTests
    {
        private readonly AbbreviationService _service;
        private readonly Mock<IAbbreviationRepository> _repository;
        private readonly Mock<ILogger<IAbbreviationService>> _logger;
        private readonly Mock<IEventPublisher> _eventPublisher;
        private readonly Mock<IConfigService> _configService;
        private readonly IMarkdownClient _markdownClient;
        public AbbreviationServiceTests() 
        {
            _repository = new Mock<IAbbreviationRepository>();
            _logger = new Mock<ILogger<IAbbreviationService>>();
            _eventPublisher = new Mock<IEventPublisher>();
            _configService = new Mock<IConfigService>();
            _markdownClient = new MarkdownClient();
            _service = new AbbreviationService(
                _repository.Object, 
                _logger.Object, 
                _configService.Object,
                _markdownClient,
                _eventPublisher.Object);
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
            Abbreviation abbreviation = new Abbreviation(name, string.Empty, string.Empty);
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == name))).ReturnsAsync(abbreviation);
            Abbreviation dbAbbreviation = await _service.GetAsync(name);
            Assert.Equal(abbreviation, dbAbbreviation);
        }
        [Fact]
        public async Task GetAll()
        {
            List<Abbreviation> abbreviations = new List<Abbreviation>()
            {
                new Abbreviation(Guid.NewGuid().ToString(), string.Empty, string.Empty),
                new Abbreviation(Guid.NewGuid().ToString(), string.Empty, string.Empty),
                new Abbreviation(Guid.NewGuid().ToString(), string.Empty, string.Empty)
            };
            _repository.Setup(x => x.GetAsync()).ReturnsAsync(abbreviations);
            List<Abbreviation> dbAbbreviations = (await _service.GetAsync()).ToList();
            Assert.Equal(dbAbbreviations, abbreviations);
        }
        [Fact]
        public async Task Delete()
        {
            string shortForm = Guid.NewGuid().ToString();
            Abbreviation abbreviation = new Abbreviation(shortForm, string.Empty, string.Empty);
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
            Abbreviation abbreviation = new Abbreviation(shortForm, string.Empty, string.Empty);
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == shortForm))).ReturnsAsync(abbreviation);
            _repository.Setup(x => x.UpdateAsync(It.Is<Abbreviation>(a => a.ShortForm == shortForm))).ReturnsAsync(abbreviation);
            Abbreviation dbAbbreviation = await _service.ChangeAsync(shortForm, new ChangeAbbreviationCmd(
                longForm,
                newDescription));
            Assert.Equal(longForm, dbAbbreviation.LongForm);
            Assert.Equal(newDescription, dbAbbreviation.Description);
            _repository.Verify(x => x.UpdateAsync(It.Is<Abbreviation>(a => a.ShortForm == shortForm)), Times.Once());
        }
        [Fact]
        public async Task Markdown()
        {
            Abbreviation abbreviation;
            Config config;
            (config, abbreviation) = SetupMarkdownHtmlTest();
            List<string> markdown = await _service.GetMarkdownAsync(abbreviation.ShortForm, config.Id);
            AssertAbbreviationInStrings(markdown, abbreviation);
        }
        [Fact]
        public async Task Html()
        {
            Abbreviation abbreviation;
            Config config;
            (config, abbreviation) = SetupMarkdownHtmlTest();
            List<string> html = await _service.GetHtmlAsync(abbreviation.ShortForm, config.Id);
            AssertAbbreviationInStrings(html, abbreviation);
            Assert.True(StringUtilities.Contains(html, "<h1>"));
        }
        private (Config, Abbreviation) SetupMarkdownHtmlTest()
        {
            Config config = new Config();
            List<Link> links = new List<Link>()
            {
                new Link($"https://{Guid.NewGuid()}.com", $"{Guid.NewGuid()}"),
                new Link($"https://{Guid.NewGuid()}.com", $"{Guid.NewGuid()}"),
            };
            _configService.Setup(x => x.GetAsync(It.Is<Guid>(id => id == config.Id))).ReturnsAsync(config);
            Abbreviation abbreviation = new Abbreviation("cd", string.Empty, string.Empty);
            abbreviation.Change(new ChangeAbbreviationCmd(
                description: "An opticial disk with encoded data",
                longForm: "Compact Disk"
            ));
            abbreviation.ChangeLinks(new ChangeAbbreviationLinksCmd(links));
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == abbreviation.Id))).ReturnsAsync(abbreviation);
            return (config, abbreviation);
        }
        private void AssertAbbreviationInStrings(IEnumerable<string> strings, Abbreviation abbreviation)
        {
            Assert.True(StringUtilities.Contains(strings, abbreviation.ShortForm));
            Assert.True(StringUtilities.Contains(strings, abbreviation.LongForm));
            foreach (var link in abbreviation.ReferenceLinks)
            {
                Assert.True(StringUtilities.Contains(strings, link.Url));
                Assert.True(StringUtilities.Contains(strings, link.LinkText));
            }
        }
    }
}
