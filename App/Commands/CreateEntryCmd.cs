using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Commands
{
    /// <summary>
    /// Create entry command
    /// </summary>
    public class CreateEntryCmd
    {
        /// <summary>
        /// Create entry command
        /// </summary>
        public CreateEntryCmd() { }

        /// <summary>
        /// Create entry command
        /// </summary>
        /// <param name="name">The entry name</param>
        public CreateEntryCmd(string name,
            IEnumerable<string> description) 
        { 
            Name = name;
            Description = description.ToList();
        }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description
        /// </summary>
        public List<string> Description { get; set; } = new List<string>();
    }
}
