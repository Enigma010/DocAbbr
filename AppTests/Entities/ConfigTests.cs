using App.Commands;
using App.Entities;
using AppEvents;
using App.Utilities;

namespace AppTests.Entities
{
    public class ConfigTests
    {
        [Fact]
        public void Create()
        {
            Config config = new Config();
            IReadOnlyCollection<object> events = config.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) => {
                Assert.IsType<ConfigCreatedEvent>(e);
                ConfigCreatedEvent configCreatedEvent = e as ConfigCreatedEvent ?? throw new InvalidCastException();
                Assert.NotEmpty(configCreatedEvent.Markdown);
            });
        }
        [Theory]
        [InlineData("12334")]
        [InlineData("abc")]
        public void SetName(string newName)
        {
            Config config = new Config();
            AssertConfigCreated(config);
            config.ChangeName(new ChangeConfigNameCmd()
            {
                Name = newName,
            });
            AssertConfigCreatedChange(config);
            Assert.Equal(newName, config.Name);
            IReadOnlyCollection<object> stateChanges = config.GetEvents();
            ConfigCreatedEvent? configCreated = stateChanges.ElementAt(0) as ConfigCreatedEvent;
            Assert.NotNull(configCreated);
            // The name of a newly created configuration is always empty
            Assert.Equal(string.Empty, configCreated.Name);
        }
        [Fact]
        public void ChangeMarkdown()
        {
            Config config = new Config();
            List<string> oldMarkdown = config.Markdown.ToList();
            List<string> newMarkdown = new List<string>()
            {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            };
            config.ClearEvents();
            config.ChangeMarkdownTemplates(new ChangeConfigMarkdownTemplateCmd(newMarkdown));
            var events = config.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) =>
            {
                var changeEvent = Assert.IsType<ConfigMarkdownTemplateChangedEvent>(e);
                Assert.True(oldMarkdown.Same(changeEvent.OldMarkdown));
                Assert.True(newMarkdown.Same(changeEvent.NewMarkdown));
            });
        }
        public static Action<object> AssertType<AssertType>()
        {
            Action<object> assert =  (sc) =>
            {
                Assert.IsType<AssertType>(sc);
            };
            return assert;
        }
        private void AssertConfigCreated(Config config)
        {
            Assert.Collection(config.GetEvents(),
                AssertType<ConfigCreatedEvent>());
        }
        private void AssertConfigCreatedChange(Config config)
        {
            Assert.Collection(
                config.GetEvents(),
                AssertType<ConfigCreatedEvent>(),
                AssertType<ConfigNameChangedEvent>());
        }
    }
}
