using App.Commands;
using App.Repositories.Dtos;
using App.Utilities;
using AppCore;
using AppEvents;

namespace App.Entities
{
    /// <summary>
    /// The configuration object
    /// </summary>
    public class Config : Entity<ConfigDto, Guid>
    {
        public const string NameTemplate = "{{name}}";
        public const string DescriptionTemplate = "{{description}}";
        public const string ReferenceLinksTemplate = "{{referenceLinks}}";
        public const string ReferenceLinkNameTemplate = "{{referenceLinkName}}";
        public const string ReferenceLinkTemplate = "{{referenceLink}}";
        /// <summary>
        /// Creates a new configuration loaded from the repository
        /// </summary>
        /// <param name="dto"></param>
        public Config(ConfigDto dto) : base(dto)
        {
        }
        /// <summary>
        /// Createa a new configuration
        /// </summary>
        public Config() : base(Guid.NewGuid)
        {
            _dto.Markdown = MarkdownTemplate();
            _dto.MarkdownReferenceLink = MarkdownReferenceLinkTemplate();
            AddEvent(new ConfigCreatedEvent(Id, Name, Markdown, MarkdownReferenceLink));
        }
        /// <summary>
        /// Createa a new configuration
        /// </summary>
        public Config(string name, bool enabled = false) : base(Guid.NewGuid)
        {
            _dto.Name = name;
            _dto.Markdown = MarkdownTemplate();
            _dto.MarkdownReferenceLink = MarkdownReferenceLinkTemplate();
            AddEvent(new ConfigCreatedEvent(Id, Name, Markdown, MarkdownReferenceLink));
        }

        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name => _dto.Name;
        
        /// <summary>
        /// Whether this is enabled or not
        /// </summary>
        public bool Enabled => _dto.Enabled;
        
        /// <summary>
        /// The markdown for an entry
        /// </summary>
        public IReadOnlyCollection<string> Markdown => (_dto.Markdown ?? new List<string>()).AsReadOnly();
        
        /// <summary>
        /// The markdown for a reference link
        /// </summary>
        public string MarkdownReferenceLink => _dto.MarkdownReferenceLink ?? string.Empty;
        
        /// <summary>
        /// Set the config to be deleted
        /// </summary>
        public override void Deleted()
        {
            AddEvent(new ConfigDeletedEvent(Id));
        }
        
        /// <summary>
        /// Chagne the configuration
        /// </summary>
        /// <param name="change">The configuration changes</param>
        public void ChangeName(ChangeConfigNameCmd change)
        {
            // Support for idempotence, this is done for future support for
            // eventing, i.e. if you event on the name change then if you 
            // call this method with the same name you don't want to trigger
            // a name change event
            string oldName = Name;
            bool changed = false;
            if (Name != change.Name)
            {
                _dto.Name = change.Name;
                changed = true;
            }
            if(changed)
            {
                AddEvent(new ConfigNameChangedEvent(Id, oldName, Name));
            }
        }
        
        /// <summary>
        /// Changes markdown templates
        /// </summary>
        /// <param name="cmd">The change markdown command</param>
        public void ChangeMarkdownTemplates(ChangeConfigMarkdownTemplateCmd cmd)
        {
            if (!Markdown.Same(cmd.Markdown) || MarkdownReferenceLink != cmd.MarkdownReferenceLink)
            {
                AddEvent(new ConfigMarkdownTemplateChangedEvent(Id, Markdown, cmd.Markdown, MarkdownReferenceLink, cmd.MarkdownReferenceLink));
                _dto.Markdown.Clear();
                _dto.Markdown.AddRange(cmd.Markdown);
                _dto.MarkdownReferenceLink = cmd.MarkdownReferenceLink;
            }
        }
        
        /// <summary>
        /// The default markdown template for the entire entry
        /// </summary>
        /// <returns>The default markdown template</returns>
        public static List<string> MarkdownTemplate()
        {
            return new List<string>()
            {
                $"# {NameTemplate}",
                $"## Description",
                $"{DescriptionTemplate}",
                $"## References",
                $"{ReferenceLinksTemplate}"
            };
        }
        
        /// <summary>
        /// Teh default markdown template for a reference link
        /// </summary>
        /// <returns>The default markdown template</returns>
        public static string MarkdownReferenceLinkTemplate()
        {
            return $"* [{ReferenceLinkNameTemplate}]({ReferenceLinkTemplate})";
        }
    }
}
