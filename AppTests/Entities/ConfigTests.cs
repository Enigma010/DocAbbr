using App.Commands;
using App.Entities;
using AppEvents;

namespace AppTests.Entities
{
    public class ConfigTests
    {
        [Theory]
        [InlineData("12334")]
        [InlineData("abc")]
        public void SetName(string newName)
        {
            Config config = new Config();
            AssertConfigCreated(config);
            bool enabled = config.Enabled;
            config.Change(new ChangeConfigCmd()
            {
                Name = newName,
                Enabled = config.Enabled
            });
            AssertConfigCreatedChange(config);
            Assert.Equal(newName, config.Name);
            Assert.Equal(enabled, config.Enabled);
            IReadOnlyCollection<object> stateChanges = config.GetEvents();
            ConfigCreatedEvent? configCreated = stateChanges.ElementAt(0) as ConfigCreatedEvent;
            Assert.NotNull(configCreated);
            // The name of a newly created configuration is always empty
            Assert.Equal(string.Empty, configCreated.Name);
            Assert.Equal(config.Enabled, configCreated.Enabled);
        }
        [Theory]
        [InlineData(true)]
        public void SetEnabled(bool enabled)
        {
            Config config = new Config();
            AssertConfigCreated(config);
            string name = config.Name;
            config.Change(new ChangeConfigCmd()
            {
                Name = config.Name,
                Enabled = enabled
            });
            AssertConfigCreatedChange(config);
            Assert.Equal(name, config.Name);
            Assert.Equal(enabled, config.Enabled);
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
                AssertType<ConfigChangedEvent>());
        }
    }
}
