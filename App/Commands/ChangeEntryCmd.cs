using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Commands
{
    /// <summary>
    /// Change a entry command
    /// </summary>
    public class ChangeEntryCmd
    {
        /// <summary>
        /// Creates a change entry command
        /// </summary>
        /// <param name="name">The nameparam>
        /// <param name="description">The description</param>
        public ChangeEntryCmd(IEnumerable<string> description)
        {
            Description = description.ToList();
        }

        /// <summary>
        /// The description
        /// </summary>
        public List<string> Description { get; set; } = new List<string>();
    }
}
