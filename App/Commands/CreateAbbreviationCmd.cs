using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Commands
{
    /// <summary>
    /// Create abbreviation command
    /// </summary>
    public class CreateAbbreviationCmd
    {
        /// <summary>
        /// Create abbreviation command
        /// </summary>
        public CreateAbbreviationCmd() { }

        /// <summary>
        /// Create abbreviation command
        /// </summary>
        /// <param name="shortForm">The abbreviation name</param>
        public CreateAbbreviationCmd(string shortForm,
            string longForm,
            IEnumerable<string> description) 
        { 
            ShortForm = shortForm;
            LongForm = longForm;
            Description = description.ToList();
        }

        /// <summary>
        /// The short form
        /// </summary>
        public string ShortForm { get; set; } = string.Empty;

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
