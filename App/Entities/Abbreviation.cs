using App.Commands;
using App.Mappers;
using App.Repositories.Dtos;
using AppCore;
using AppEvents;
using Markdig.Renderers.Html;
using MassTransit.SagaStateMachine;
using MongoDB.Bson.Serialization.Attributes;
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
    public class Abbreviation : Entity<AbbreviationDto, string>
    {
        /// <summary>
        /// Creates a default abbreviation
        /// </summary>
        public Abbreviation() : base(() => { return string.Empty;  })
        {
        }

        /// <summary>
        /// Creates an abbreviation
        /// </summary>
        /// <param name="dto">The data transfer object</param>
        public Abbreviation(AbbreviationDto dto): base(dto) 
        { 
        }

        /// <summary>
        /// Creates a new description
        /// </summary>
        /// <param name="shortForm">The short form, i.e. cd</param>
        /// <param name="longForm">The long form, i.e. compact disk</param>
        /// <param name="description">The description</param>
        public Abbreviation(string shortForm, 
            string longForm, 
            string description) : base(() => { return shortForm; })
        {
            AddEvent(new AbbreviationCreatedEvent(shortForm, longForm, description));
        }

        /// <summary>
        /// Gets the short form of the abbreviation, for example CD
        /// for compact disk
        /// </summary>
        public string ShortForm => _dto.Id;

        /// <summary>
        /// Gets the long form of the abbreviation, for example
        /// compact disk for CD
        /// </summary>
        public string LongForm => _dto.LongForm;
        
        /// <summary>
        /// The description of the abbreviation
        /// </summary>
        public string Description => _dto.Description;
        
        /// <summary>
        /// The reference links
        /// </summary>
        public IReadOnlyCollection<Link> ReferenceLinks => _dto.ReferenceLinks.Select(rl => new Link(rl)).ToList().AsReadOnly();
        
        /// <summary>
        /// Changes an abbreviation
        /// </summary>
        /// <param name="cmd">The change abbreviation command</param>
        public void Change(ChangeAbbreviationCmd cmd)
        {
            if (LongForm != cmd.LongForm || Description != cmd.Description)
            {
                AddEvent(new AbbreviationChangedEvent(LongForm, cmd.LongForm, Description, cmd.Description));
                _dto.LongForm = cmd.LongForm;
                _dto.Description = cmd.Description;
            }
        }
        
        /// <summary>
        /// Changes the links related to the abbreviation
        /// </summary>
        /// <param name="newReferenceLinks"></param>
        public void ChangeLinks(ChangeAbbreviationLinksCmd cmd)
        {
            List<LinkDto> addedReferenceLinks = LinkDto.Added(_dto.ReferenceLinks, cmd.Links).ToList();
            List<LinkDto> changedReferenceLinks = LinkDto.Changed(_dto.ReferenceLinks, cmd.Links).ToList();
            List<LinkDto> deletedReferenceLinks = LinkDto.Deleted(_dto.ReferenceLinks, cmd.Links).ToList();
            if (addedReferenceLinks.Count > 0 || changedReferenceLinks.Count > 0 || deletedReferenceLinks.Count > 0)
            {
                AddEvent(new AbbreviationReferenceLinksChangedEvent(addedReferenceLinks.Map(), changedReferenceLinks.Map(), deletedReferenceLinks.Map()));
                _dto.ReferenceLinks.Clear();
                _dto.ReferenceLinks.AddRange(cmd.Links.Select(link => new LinkDto(link.Url, link.LinkText)));
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
                foreach (var referenceLink in _dto.ReferenceLinks)
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

        /// <summary>
        /// Delete an abbreviation
        /// </summary>
        public override void Deleted()
        {
            AddEvent(new AbbreviationDeletedEvent(
                _dto.ShortForm,
                _dto.LongForm,
                _dto.Description,
                _dto.ReferenceLinks.Select(
                    rl => 
                    new Tuple<string, string>(
                        rl.Url, 
                        rl.LinkText)).ToList()));
        }
    }
}
