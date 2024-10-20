using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Commands
{
    /// <summary>
    /// The markdown template change command
    /// </summary>
    public class ChangeConfigMarkdownTemplateCmd
    {
        /// <summary>
        /// Creates a markdown template change command
        /// </summary>
        /// <param name="markdownTemplate">The new markdown template</param>
        public ChangeConfigMarkdownTemplateCmd(List<string> markdownTemplate) 
        { 
            Markdown = markdownTemplate;
        }

        /// <summary>
        /// The markdown
        /// </summary>
        public List<string> Markdown
        {
            get;
            set;
        } = new List<string>();
    }
}
