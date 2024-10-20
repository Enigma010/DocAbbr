using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvents
{
    /// <summary>
    /// Configuration markdown template changed event
    /// </summary>
    public class ConfigMarkdownTemplateChangedEvent
    {
        /// <summary>
        /// Create configuration markdown template changed event
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="markdown">The markdown</param>
        /// <param name="markdownReferenceLink"></param>
        public ConfigMarkdownTemplateChangedEvent(
            Guid id,
            IEnumerable<string> oldMarkdown,
            IEnumerable<string> newMarkdown)
        {
            Id = id;
            OldMarkdown = oldMarkdown.ToList();
            NewMarkdown = newMarkdown.ToList();
        }

        /// <summary>
        /// The ID
        /// </summary>
        public Guid Id 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The old markdown
        /// </summary>
        public List<string> OldMarkdown
        {
            get;
            set;
        } = new List<string>();

        /// <summary>
        /// The new markdown
        /// </summary>
        public List<string> NewMarkdown
        {
            get;
            set;
        } = new List<string>();
    }
}
