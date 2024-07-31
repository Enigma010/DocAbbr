using App.Commands;
using App.Entities;
using AppEvents;
using App.Utilities;
using System.Net.Sockets;
using App.Mappers;

namespace AppTests.Entities
{
    public class AbbreviationTests
    {
        public AbbreviationTests() 
        { 
        }
        [Fact]
        public void New()
        {
            string name = "CD";
            Abbreviation abbreviation = new Abbreviation(name, string.Empty, string.Empty);
            IReadOnlyCollection<object> events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) =>
            {
                Assert.IsType<AbbreviationCreatedEvent>(e);
                AbbreviationCreatedEvent createdEvent = e as AbbreviationCreatedEvent ?? throw new ArgumentNullException();
                Assert.NotNull(createdEvent);
                Assert.Equal(name, createdEvent.ShortForm);
            });
        }
        [Fact]
        public void NewWithLinks()
        {
            string name = "CD";
            ChangeAbbreviationLinksCmd cmd = new ChangeAbbreviationLinksCmd(new List<ChangeLinkCmd>()
            {
                new ChangeLinkCmd()
                {
                    Url = @$"https://{Guid.NewGuid()}.com",
                    LinkText = Guid.NewGuid().ToString()
                }
            });
            Abbreviation abbreviation = new Abbreviation(name, string.Empty, string.Empty);
            abbreviation.ChangeLinks(cmd);
            var events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events,
                (e) =>
                {
                    Assert.IsType<AbbreviationCreatedEvent>(e);
                },
                (e) => 
                { 
                    Assert.IsType<AbbreviationReferenceLinksChangedEvent>(e);
                }
            );
        }
        [Fact]
        public void ChangeName()
        {
            Abbreviation abbreviation = new Abbreviation("CD", string.Empty, string.Empty);
            abbreviation.ClearEvents();
            string oldFullName = abbreviation.LongForm;
            string oldDescription = abbreviation.Description;
            string newFullName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();
            abbreviation.Change(new ChangeAbbreviationCmd(newFullName, newDescription));
            IReadOnlyCollection<object> events = abbreviation.GetEvents();
            Assert.Collection(events, (e) =>
            {
                Assert.IsType<AbbreviationChangedEvent>(e);
                AbbreviationChangedEvent changedEvent = e as AbbreviationChangedEvent ?? throw new ArgumentNullException();
                Assert.NotNull(changedEvent);
                Assert.Equal(oldFullName, changedEvent.OldFullName);
                Assert.Equal(newFullName, changedEvent.NewFullName);
                Assert.Equal(oldDescription, changedEvent.OldDescription);
                Assert.Equal(newDescription, changedEvent.NewDescription);
            });
        }
        [Fact]
        public void Mardown()
        {
            Config config = new Config();
            Abbreviation abbreviation = CdAbbreviation();
            List<string> markdown = abbreviation.Markdown(config).ToList();
            foreach(string find in new string[] { abbreviation.ShortForm, abbreviation.LongForm, abbreviation.Description })
            {
                Assert.True(StringUtilities.Contains(markdown, find));
            }
            foreach(App.Entities.Link link in abbreviation.ReferenceLinks)
            {
                Assert.True(StringUtilities.Contains(markdown, link.Url));
                Assert.True(StringUtilities.Contains(markdown, link.LinkText));
            }
        }
        [Fact]
        public void ChangeFullName()
        {
            Config config = new Config();
            Abbreviation abbreviation = CdAbbreviation();
            string newFullName = Guid.NewGuid().ToString();
            abbreviation.Change(new ChangeAbbreviationCmd(newFullName, abbreviation.Description));
            List<string> markdown = abbreviation.Markdown(config).ToList();
            Assert.True(StringUtilities.Contains(markdown, newFullName));
        }
        [Fact]
        public void ChangeDescription()
        {
            Config config = new Config();
            Abbreviation abbreviation = CdAbbreviation();
            string description = Guid.NewGuid().ToString();
            abbreviation.Change(new ChangeAbbreviationCmd(abbreviation.LongForm, description));
            List<string> markdown = abbreviation.Markdown(config).ToList();
            Assert.True(StringUtilities.Contains(markdown, description));
        }
        [Fact]
        public void AddReferenceLink()
        {
            Config config = new Config();
            Abbreviation abbreviation = CdAbbreviation();
            abbreviation.ClearEvents();
            List<App.Entities.Link> links = abbreviation.ReferenceLinks.ToList();
            links.Add(new App.Entities.Link($"https://{Guid.NewGuid()}.domain.com", Guid.NewGuid().ToString()));
            abbreviation.ChangeLinks(new ChangeAbbreviationLinksCmd(links));
            Assert.Equal(links.Count, abbreviation.ReferenceLinks.Count);
            foreach(var link in links)
            {
                Assert.Contains(abbreviation.ReferenceLinks, rl => rl.Url == link.Url && rl.LinkText == link.LinkText);
            }
            IReadOnlyCollection<object> events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) =>
            {
                var linksChangedEvent = Assert.IsType<AbbreviationReferenceLinksChangedEvent>(e);
                Assert.NotEmpty(linksChangedEvent.AddedReferenceLinks);
                Assert.Empty(linksChangedEvent.ChangedReferenceLinks);
                Assert.Empty(linksChangedEvent.DeletedReferenceLinks);
            });
        }
        [Fact]
        public void DeleteReferenceLink()
        {
            Config config = new Config();
            Abbreviation abbreviation = CdAbbreviation();
            abbreviation.ClearEvents();
            List<App.Entities.Link> links = abbreviation.ReferenceLinks.ToList();
            links.RemoveAt(1);
            abbreviation.ChangeLinks(new ChangeAbbreviationLinksCmd(links));
            Assert.Equal(links.Count, abbreviation.ReferenceLinks.Count);
            foreach (var link in links)
            {
                Assert.Contains(abbreviation.ReferenceLinks, rl => rl.Url == link.Url && rl.LinkText == link.LinkText);
            }
            IReadOnlyCollection<object> events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) =>
            {
                var linksChangedEvent = Assert.IsType<AbbreviationReferenceLinksChangedEvent>(e);
                Assert.Empty(linksChangedEvent.AddedReferenceLinks);
                Assert.Empty(linksChangedEvent.ChangedReferenceLinks);
                Assert.NotEmpty(linksChangedEvent.DeletedReferenceLinks);
            });
        }
        [Fact]
        public void ChangeReferenceLink()
        {
            Config config = new Config();
            Abbreviation abbreviation = CdAbbreviation();
            abbreviation.ClearEvents();
            List<App.Entities.Link> changedLinks = abbreviation.ReferenceLinks.ToList();
            App.Entities.Link changedLink = new App.Entities.Link(changedLinks[1].Url, Guid.NewGuid().ToString());
            changedLinks.RemoveAt(1);
            changedLinks.Add(changedLink);
            abbreviation.ChangeLinks(new ChangeAbbreviationLinksCmd(changedLinks));
            Assert.Equal(changedLinks.Count, abbreviation.ReferenceLinks.Count);
            foreach (var link in changedLinks)
            {
                Assert.Contains(abbreviation.ReferenceLinks, rl => rl.Url == link.Url && rl.LinkText == link.LinkText);
            }
            IReadOnlyCollection<object> events = abbreviation.GetEvents();
            Assert.NotEmpty(events);
            Assert.Collection(events, (e) =>
            {
                var linksChangedEvent = Assert.IsType<AbbreviationReferenceLinksChangedEvent>(e);
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
        public static Abbreviation CdAbbreviation()
        {
            string abbreviationString = "CD";
            string fullName = "Compact Disk";
            string description = "A flat plate with optical data on it";
            List<App.Entities.Link> links = new List<App.Entities.Link>();
            links.Add(new App.Entities.Link($"https://{Guid.NewGuid()}.domain.com", Guid.NewGuid().ToString()));
            links.Add(new App.Entities.Link($"https://{Guid.NewGuid()}.domain.com", Guid.NewGuid().ToString()));
            Abbreviation abbreviation = new Abbreviation(abbreviationString, string.Empty, string.Empty);
            abbreviation.Change(new App.Commands.ChangeAbbreviationCmd(fullName, description));
            abbreviation.ChangeLinks(new ChangeAbbreviationLinksCmd(links));
            return abbreviation;
        }
    }
}
