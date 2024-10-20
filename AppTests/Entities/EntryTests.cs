using App.Commands;
using App.Entities;
using AppEvents;
using App.Utilities;
using System.Net.Sockets;
using App.Mappers;

namespace AppTests.Entities
{
    public class EntryTests
    {
        public EntryTests() 
        { 
        }

        [Fact]
        public void New()
        {
            string name = "CD";
            Entry abbreviation = new Entry(name, Array.Empty<string>());
            IReadOnlyCollection<object> events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) =>
            {
                Assert.IsType<EntryCreatedEvent>(e);
                EntryCreatedEvent createdEvent = e as EntryCreatedEvent ?? throw new ArgumentNullException();
                Assert.NotNull(createdEvent);
                Assert.Equal(name, createdEvent.Name);
            });
        }

        [Fact]
        public void ChangeName()
        {
            Entry entry = new Entry("CD", new List<string>{ });
            entry.ClearEvents();
            List<string> oldDescription = entry.Description.ToList();
            string newFullName = Guid.NewGuid().ToString();
            List<string> newDescription = new List<string>() { Guid.NewGuid().ToString() };
            entry.Change(new ChangeEntryCmd(newDescription));
            IReadOnlyCollection<object> events = entry.GetEvents();
            Assert.Collection(events, (e) =>
            {
                Assert.IsType<EntryChangedEvent>(e);
                EntryChangedEvent changedEvent = e as EntryChangedEvent ?? throw new ArgumentNullException();
                Assert.NotNull(changedEvent);
                Assert.Equal(oldDescription, changedEvent.OldDescription);
                Assert.Equal(newDescription, changedEvent.NewDescription);
            });
        }

        [Fact]
        public void Mardown()
        {
            Config config = new Config();
            Entry entry = CdEntry();
            List<string> markdown = entry.Markdown(config).ToList();
            foreach(string find in new string[] { entry.Name })
            {
                Assert.True(StringUtilities.Contains(markdown, find));
            }
            foreach(string find in entry.Description)
            {
                Assert.True(StringUtilities.Contains(markdown, find));
            }
        }

        [Fact]
        public void ChangeDescription()
        {
            Config config = new Config();
            Entry entry = CdEntry();
            List<string> description = new List<string>() { Guid.NewGuid().ToString() };
            entry.Change(new ChangeEntryCmd(description));
            List<string> markdown = entry.Markdown(config).ToList();
            Assert.True(StringUtilities.Contains(markdown, description));
        }

        public static Entry CdEntry()
        {
            string name = "CD";
            List<string> description = new List<string>() { "A flat plate with optical data on it" };
            Entry abbreviation = new Entry(name, Array.Empty<string>());
            abbreviation.Change(new App.Commands.ChangeEntryCmd(description));
            return abbreviation;
        }
    }
}
