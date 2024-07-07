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
        /// <param name="markdown">The markdown</param>
        /// <param name="markdownReferenceLink"></param>
        public ConfigMarkdownTemplateChangedEvent(IEnumerable<string> oldMarkdown,
            IEnumerable<string> newMarkdown, 
            string oldMarkdownReferenceLink,
            string newMarkdownReferenceLink)
        {
            OldMarkdown = oldMarkdown.ToList();
            NewMarkdown = newMarkdown.ToList();
            OldMarkdownReferenceLink = oldMarkdownReferenceLink;
            NewMarkdownReferenceLink = newMarkdownReferenceLink;
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
        /// <summary>
        /// The old markdown reference link
        /// </summary>
        public string OldMarkdownReferenceLink
        {
            get;
            set;
        } = string.Empty;
        /// <summary>
        /// The new markdown reference link
        /// </summary>
        public string NewMarkdownReferenceLink 
        { 
            get; 
            set; 
        } = string.Empty;
    }
}
