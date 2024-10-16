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
        public void NewWithLinks()
        {
            string name = "CD";
            ChangeEntryLinksCmd cmd = new ChangeEntryLinksCmd(new List<ChangeLinkCmd>()
            {
                new ChangeLinkCmd()
                {
                    Url = @$"https://{Guid.NewGuid()}.com",
                    LinkText = Guid.NewGuid().ToString()
                }
            });
            Entry abbreviation = new Entry(name, Array.Empty<string>());
            abbreviation.ChangeLinks(cmd);
            var events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events,
                (e) =>
                {
                    Assert.IsType<EntryCreatedEvent>(e);
                },
                (e) => 
                { 
                    Assert.IsType<EntryReferenceLinksChangedEvent>(e);
                }
            );
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
            foreach(App.Entities.Link link in entry.ReferenceLinks)
            {
                Assert.True(StringUtilities.Contains(markdown, link.Url));
                Assert.True(StringUtilities.Contains(markdown, link.LinkText));
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

        [Fact]
        public void AddReferenceLink()
        {
            Config config = new Config();
            Entry abbreviation = CdEntry();
            abbreviation.ClearEvents();
            List<App.Entities.Link> links = abbreviation.ReferenceLinks.ToList();
            links.Add(new App.Entities.Link($"https://{Guid.NewGuid()}.domain.com", Guid.NewGuid().ToString()));
            abbreviation.ChangeLinks(new ChangeEntryLinksCmd(links));
            Assert.Equal(links.Count, abbreviation.ReferenceLinks.Count);
            foreach(var link in links)
            {
                Assert.Contains(abbreviation.ReferenceLinks, rl => rl.Url == link.Url && rl.LinkText == link.LinkText);
            }
            IReadOnlyCollection<object> events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) =>
            {
                var linksChangedEvent = Assert.IsType<EntryReferenceLinksChangedEvent>(e);
                Assert.NotEmpty(linksChangedEvent.AddedReferenceLinks);
                Assert.Empty(linksChangedEvent.ChangedReferenceLinks);
                Assert.Empty(linksChangedEvent.DeletedReferenceLinks);
            });
        }

        [Fact]
        public void DeleteReferenceLink()
        {
            Config config = new Config();
            Entry abbreviation = CdEntry();
            abbreviation.ClearEvents();
            List<App.Entities.Link> links = abbreviation.ReferenceLinks.ToList();
            links.RemoveAt(1);
            abbreviation.ChangeLinks(new ChangeEntryLinksCmd(links));
            Assert.Equal(links.Count, abbreviation.ReferenceLinks.Count);
            foreach (var link in links)
            {
                Assert.Contains(abbreviation.ReferenceLinks, rl => rl.Url == link.Url && rl.LinkText == link.LinkText);
            }
            IReadOnlyCollection<object> events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) =>
            {
                var linksChangedEvent = Assert.IsType<EntryReferenceLinksChangedEvent>(e);
                Assert.Empty(linksChangedEvent.AddedReferenceLinks);
                Assert.Empty(linksChangedEvent.ChangedReferenceLinks);
                Assert.NotEmpty(linksChangedEvent.DeletedReferenceLinks);
            });
        }

        [Fact]
        public void ChangeReferenceLink()
        {
            Config config = new Config();
            Entry abbreviation = CdEntry();
            abbreviation.ClearEvents();
            List<App.Entities.Link> changedLinks = abbreviation.ReferenceLinks.ToList();
            App.Entities.Link changedLink = new App.Entities.Link(changedLinks[1].Url, Guid.NewGuid().ToString());
            changedLinks.RemoveAt(1);
            changedLinks.Add(changedLink);
            abbreviation.ChangeLinks(new ChangeEntryLinksCmd(changedLinks));
            Assert.Equal(changedLinks.Count, abbreviation.ReferenceLinks.Count);
            foreach (var link in changedLinks)
            {
                Assert.Contains(abbreviation.ReferenceLinks, rl => rl.Url == link.Url && rl.LinkText == link.LinkText);
            }
            IReadOnlyCollection<object> events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) =>
            {
                var linksChangedEvent = Assert.IsType<EntryReferenceLinksChangedEvent>(e);
                Assert.Empty(linksChangedEvent.AddedReferenceLinks);
                Assert.NotEmpty(linksChangedEvent.ChangedReferenceLinks);
                Assert.Single(linksChangedEvent.ChangedReferenceLinks);
                Assert.Collection(linksChangedEvent.ChangedReferenceLinks, (e2) =>
                {
                    Assert.Equal(changedLink.Url, e2.Url);
                    Assert.Equal(changedLink.LinkText, e2.LinkText);
                });
                Assert.Empty(linksChangedEvent.DeletedReferenceLinks);
            });
        }

        public static Entry CdEntry()
        {
            string name = "CD";
            List<string> description = new List<string>() { "A flat plate with optical data on it" };
            List<App.Entities.Link> links = new List<App.Entities.Link>();
            links.Add(new App.Entities.Link($"https://{Guid.NewGuid()}.domain.com", Guid.NewGuid().ToString()));
            links.Add(new App.Entities.Link($"https://{Guid.NewGuid()}.domain.com", Guid.NewGuid().ToString()));
            Entry abbreviation = new Entry(name, Array.Empty<string>());
            abbreviation.Change(new App.Commands.ChangeEntryCmd(description));
            abbreviation.ChangeLinks(new ChangeEntryLinksCmd(links));
            return abbreviation;
        }
    }
}
