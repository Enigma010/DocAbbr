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
    public class EntryServiceTests
    {
        private readonly EntryService _service;
        private readonly Mock<IEntryRepository> _repository;
        private readonly Mock<ILogger<IEntryService>> _logger;
        private readonly Mock<IEventPublisher> _eventPublisher;
        private readonly Mock<IConfigService> _configService;
        private readonly IMarkdownClient _markdownClient;
        public EntryServiceTests() 
        {
            _repository = new Mock<IEntryRepository>();
            _logger = new Mock<ILogger<IEntryService>>();
            _eventPublisher = new Mock<IEventPublisher>();
            _configService = new Mock<IConfigService>();
            _markdownClient = new MarkdownClient();
            _service = new EntryService(
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
            Entry abbreviation = await _service.CreateAsync(new CreateEntryCmd(shortForm, Array.Empty<string>()));
            Assert.Equal(shortForm, abbreviation.Name);
            _repository.Verify(e => e.InsertAsync(It.Is<Entry>(a => a.Id == shortForm)), Times.Once);
        }
        [Fact]
        public async Task Get()
        {
            string name = Guid.NewGuid().ToString();
            Entry abbreviation = new Entry(name, Array.Empty<string>());
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == name))).ReturnsAsync(abbreviation);
            Entry dbAbbreviation = await _service.GetAsync(name);
            Assert.Equal(abbreviation, dbAbbreviation);
        }
        [Fact]
        public async Task GetAll()
        {
            List<Entry> abbreviations = new List<Entry>()
            {
                new Entry(Guid.NewGuid().ToString(), Array.Empty<string>()),
                new Entry(Guid.NewGuid().ToString(), Array.Empty<string>()),
                new Entry(Guid.NewGuid().ToString(), Array.Empty<string>())
            };
            _repository.Setup(x => x.GetAsync()).ReturnsAsync(abbreviations);
            List<Entry> dbAbbreviations = (await _service.GetAsync()).ToList();
            Assert.Equal(dbAbbreviations, abbreviations);
        }
        [Fact]
        public async Task Delete()
        {
            string shortForm = Guid.NewGuid().ToString();
            Entry abbreviation = new Entry(shortForm, Array.Empty<string>());
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == shortForm))).ReturnsAsync(abbreviation);
            await _service.DeleteAsync(shortForm);
            _repository.Verify(x => x.DeleteAsync(It.Is<Entry>(a => a.Name == shortForm)), Times.Once());
        }
        [Fact]
        public async Task Change()
        {
            string name = Guid.NewGuid().ToString();
            List<string> newDescription = new List<string>() { Guid.NewGuid().ToString() };
            Entry entry = new Entry(name, Array.Empty<string>());
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == name))).ReturnsAsync(entry);
            _repository.Setup(x => x.UpdateAsync(It.Is<Entry>(a => a.Name == name))).ReturnsAsync(entry);
            Entry dbEntry = await _service.ChangeAsync(name, new ChangeEntryCmd(newDescription));
            Assert.Equal(name, dbEntry.Name);
            Assert.Equal(newDescription, dbEntry.Description);
            _repository.Verify(x => x.UpdateAsync(It.Is<Entry>(a => a.Name == name)), Times.Once());
        }
        [Fact]
        public async Task Markdown()
        {
            Entry abbreviation;
            Config config;
            (config, abbreviation) = SetupMarkdownHtmlTest();
            List<string> markdown = await _service.GetMarkdownAsync(abbreviation.Name, config.Id);
            AssertAbbreviationInStrings(markdown, abbreviation);
        }
        [Fact]
        public async Task Html()
        {
            Entry abbreviation;
            Config config;
            (config, abbreviation) = SetupMarkdownHtmlTest();
            List<string> html = await _service.GetHtmlAsync(abbreviation.Name, config.Id);
            AssertAbbreviationInStrings(html, abbreviation);
            Assert.True(StringUtilities.Contains(html, "<h1>"));
        }
        private (Config, Entry) SetupMarkdownHtmlTest()
        {
            Config config = new Config();
            List<Link> links = new List<Link>()
            {
                new Link($"https://{Guid.NewGuid()}.com", $"{Guid.NewGuid()}"),
                new Link($"https://{Guid.NewGuid()}.com", $"{Guid.NewGuid()}"),
            };
            _configService.Setup(x => x.GetAsync(It.Is<Guid>(id => id == config.Id))).ReturnsAsync(config);
            Entry abbreviation = new Entry("cd", Array.Empty<string>());
            abbreviation.Change(new ChangeEntryCmd(
                description: new List<string>() { "An opticial disk with encoded data" }
            ));
            abbreviation.ChangeLinks(new ChangeEntryLinksCmd(links));
            _repository.Setup(x => x.GetAsync(It.Is<string>(id => id == abbreviation.Id))).ReturnsAsync(abbreviation);
            return (config, abbreviation);
        }
        private void AssertAbbreviationInStrings(IEnumerable<string> strings, Entry abbreviation)
        {
            Assert.True(StringUtilities.Contains(strings, abbreviation.Name));
            foreach (var link in abbreviation.ReferenceLinks)
            {
                Assert.True(StringUtilities.Contains(strings, link.Url));
                Assert.True(StringUtilities.Contains(strings, link.LinkText));
            }
        }
    }
}
