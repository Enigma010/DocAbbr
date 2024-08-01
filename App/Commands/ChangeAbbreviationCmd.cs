using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Commands
{
    /// <summary>
    /// Change a abbreviation command
    /// </summary>
    public class ChangeAbbreviationCmd
    {
        /// <summary>
        /// Creates a change abbreviation command
        /// </summary>
        /// <param name="longForm">The long form/param>
        /// <param name="description">The description</param>
        public ChangeAbbreviationCmd(string longForm, IEnumerable<string> description)
        {
            LongForm = longForm;
            Description = description.ToList();
        }

        /// <summary>
        /// The long form
        /// </summary>
        public string LongForm { get; set; } = string.Empty;

        /// <summary>
        /// The description
        /// </summary>
        public List<string> Description { get; set; } = new List<string>();
    }
}
