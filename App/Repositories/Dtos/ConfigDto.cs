using AppCore;
using AppEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Dtos
{
    public class ConfigDto : EntityDto<Guid>
    {
        /// <summary>
        /// Creates a new configuration
        /// </summary>
        /// <param name="idFunc">The function to generate a new ID</param>
        public ConfigDto(Func<Guid> idFunc) : base(idFunc)
        {
        }
        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Whether the configuration is active or not
        /// </summary>
        public bool Enabled { get; set; } = false;
        /// <summary>
        /// The markdown for the entry
        /// </summary>
        public List<string> Markdown { get; set; } = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        public string MarkdownReferenceLink { get; set; } = string.Empty;
    }
}
