namespace AppEvents
{
    /// <summary>
    /// Config created state
    /// </summary>
    public class ConfigCreatedEvent
    {
        /// <summary>
        /// Config created state
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="name">The name</param>
        /// <param name="enabled">Whether the configuration is enabled or not</param>
        /// <param name="markdown">The markdown</param>
        public ConfigCreatedEvent(Guid id, 
            string name, 
            IEnumerable<string> markdown, 
            string markdownReferenceLink)
        {
            Id = id;
            Name = name;
            Markdown = markdown.ToList();
            MarkdownReferenceLink = markdownReferenceLink;
        }
        /// <summary>
        /// The ID of the config
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name
        {
            get;
            set;
        } = string.Empty;
        
        /// <summary>
        /// The markdown
        /// </summary>
        public List<string> Markdown
        {
            get;
            set;
        } = new List<string>();
        
        /// <summary>
        /// The markdown for a reference link
        /// </summary>
        public string MarkdownReferenceLink
        {
            get;
            set;
        } = string.Empty;
    }
}
