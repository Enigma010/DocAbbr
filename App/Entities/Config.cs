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
            AddEvent(new ConfigCreatedEvent(Id, Name, Markdown));
        }
        /// <summary>
        /// Createa a new configuration
        /// </summary>
        public Config(string name, bool enabled = false) : base(Guid.NewGuid)
        {
            _dto.Name = name;
            _dto.Markdown = MarkdownTemplate();
            AddEvent(new ConfigCreatedEvent(Id, Name, Markdown));
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
            if (!Markdown.Same(cmd.Markdown))
            {
                AddEvent(new ConfigMarkdownTemplateChangedEvent(Id, Markdown, cmd.Markdown));
                _dto.Markdown.Clear();
                _dto.Markdown.AddRange(cmd.Markdown);
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
            };
        }
    }
}
