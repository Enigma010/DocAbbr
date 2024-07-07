using App.Commands;
using App.Utilities;
using AppCore;
using AppEvents;

namespace App.Entities
{
    /// <summary>
    /// The configuration object
    /// </summary>
    public class Config : Entity<Guid>
    {
        public const string ShortFormTemplate = "{{shortForm}}";
        public const string LongFormTemplate = "{{longForm}}";
        public const string DescriptionTemplate = "{{description}}";
        public const string ReferenceLinksTemplate = "{{referenceLinks}}";
        public const string ReferenceLinkNameTemplate = "{{referenceLinkName}}";
        public const string ReferenceLinkTemplate = "{{referenceLink}}";
        private readonly List<string> _markdown = new List<string>();
        /// <summary>
        /// Createa a new configuration
        /// </summary>
        public Config() : base(Guid.NewGuid)
        {
            _markdown = MarkdownTemplate();
            MarkdownReferenceLink = MarkdownReferenceLinkTemplate();
            AddEvent(new ConfigCreatedEvent(Id, Name, Markdown, MarkdownReferenceLink));
        }
        /// <summary>
        /// Createa a new configuration
        /// </summary>
        public Config(string name, bool enabled = false) : base(Guid.NewGuid)
        {
            Name = name;
            _markdown = MarkdownTemplate();
            MarkdownReferenceLink = MarkdownReferenceLinkTemplate();
            AddEvent(new ConfigCreatedEvent(Id, Name, Markdown, MarkdownReferenceLink));
        }
        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>
        /// The markdown for an abbreviation
        /// </summary>
        public IReadOnlyCollection<string> Markdown
        {
            get
            {
                return _markdown.AsReadOnly();
            }
        }
        /// <summary>
        /// The markdown for a reference link
        /// </summary>
        public string MarkdownReferenceLink { get; private set; } = string.Empty;
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
                Name = change.Name;
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
                AddEvent(new ConfigMarkdownTemplateChangedEvent(Markdown, cmd.Markdown, MarkdownReferenceLink, cmd.MarkdownReferenceLink));
                _markdown.Clear();
                _markdown.AddRange(cmd.Markdown);
                MarkdownReferenceLink = cmd.MarkdownReferenceLink;
            }
        }
        /// <summary>
        /// The default markdown template for the entire abbreviation
        /// </summary>
        /// <returns>The default markdown template</returns>
        public static List<string> MarkdownTemplate()
        {
            return new List<string>()
            {
                $"# {ShortFormTemplate}",
                $"## Full Name",
                $"{LongFormTemplate}",
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
