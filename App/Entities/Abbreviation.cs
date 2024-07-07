using App.Commands;
using App.Mappers;
using AppCore;
using AppEvents;
using MassTransit.SagaStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities
{
    /// <summary>
    /// An abbreviation
    /// </summary>
    public class Abbreviation : Entity<string>
    {
        private List<Link> _referenceLinks = new List<Link>();
        /// <summary>
        /// Creates a new description
        /// </summary>
        /// <param name="getNewId"></param>
        public Abbreviation(string name) : base(() => { return name; })
        {
            _referenceLinks = new List<Link>();
            AddEvent(new AbbreviationCreatedEvent(name));
        }
        public Abbreviation(string name, IEnumerable<Link> referenceLinks) : base(() => { return name; })
        {

            _referenceLinks = referenceLinks.ToList();
            AddEvent(new AbbreviationCreatedEvent(name));
            AddEvent(new AbbreviationReferenceLinksChangedEvent(referenceLinks.Map(), new AppEvents.Link[] { }, new AppEvents.Link[] { }));
        }
        /// <summary>
        /// Gets the short form of the abbreviation, for example CD
        /// for compact disk
        /// </summary>
        public string ShortForm
        {
            get
            {
                return Id;
            }
        } 
        /// <summary>
        /// Gets the long form of the abbreviation, for example
        /// compact disk for CD
        /// </summary>
        public string LongForm
        {
            get;
            private set;
        } = string.Empty;
        /// <summary>
        /// The description of the abbreviation
        /// </summary>
        public string Description
        {
            get;
            private set;
        } = string.Empty;
        /// <summary>
        /// The links
        /// </summary>
        public IReadOnlyCollection<Link> ReferenceLinks
        {
            get
            {
                return _referenceLinks.AsReadOnly();
            }
        }
        /// <summary>
        /// Changes an abbreviation
        /// </summary>
        /// <param name="cmd">The change abbreviation command</param>
        public void Change(ChangeAbbreviationCmd cmd)
        {
            if (LongForm != cmd.LongForm || Description != cmd.Description)
            {
                AddEvent(new AbbreviationChangedEvent(LongForm, cmd.LongForm, Description, cmd.Description));
                LongForm = cmd.LongForm;
                Description = cmd.Description;
            }
        }
        /// <summary>
        /// Changes the links related to the abbreviation
        /// </summary>
        /// <param name="newReferenceLinks"></param>
        public void ChangeLinks(ChangeAbbreviationLinksCmd cmd)
        {
            List<Link> addedReferenceLinks = Link.Added(ReferenceLinks, cmd.Links).ToList();
            List<Link> changedReferenceLinks = Link.Changed(ReferenceLinks, cmd.Links).ToList();
            List<Link> deletedReferenceLinks = Link.Deleted(ReferenceLinks, cmd.Links).ToList();
            if (addedReferenceLinks.Count > 0 || changedReferenceLinks.Count > 0 || deletedReferenceLinks.Count > 0)
            {
                AddEvent(new AbbreviationReferenceLinksChangedEvent(addedReferenceLinks.Map(), changedReferenceLinks.Map(), deletedReferenceLinks.Map()));
                _referenceLinks = cmd.Links.ToList();
            }
        }
        /// <summary>
        /// Gets the markdown
        /// </summary>
        /// <param name="config">The configuraiton</param>
        /// <returns>The markdown</returns>
        public IEnumerable<string> Markdown(Config config)
        {
            StringBuilder markdown = new StringBuilder(string.Join('\n', config.Markdown));
            markdown = markdown.Replace(Config.ShortFormTemplate, ShortForm);
            markdown = markdown.Replace(Config.LongFormTemplate, LongForm);
            markdown = markdown.Replace(Config.DescriptionTemplate, Description);
            StringBuilder referenceLinksMarkdown = new StringBuilder();
            if (!string.IsNullOrEmpty(config.MarkdownReferenceLink))
            {
                foreach (var referenceLink in ReferenceLinks)
                {
                    referenceLinksMarkdown.Append(
                        config.MarkdownReferenceLink.Replace(
                            Config.ReferenceLinkNameTemplate, 
                            referenceLink.LinkText).Replace(Config.ReferenceLinkTemplate, referenceLink.Url) + "\n");
                }
                markdown = markdown.Replace(Config.ReferenceLinksTemplate, referenceLinksMarkdown.ToString());
            }
            return markdown.ToString().Split('\n');
        }
    }
}
