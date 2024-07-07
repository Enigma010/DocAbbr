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
        /// The create abbreviation command
        /// </summary>
        /// <param name="shortForm">The abbreviation name</param>
        public CreateAbbreviationCmd(string shortForm) 
        { 
            ShortForm = shortForm;
        }
        /// <summary>
        /// The short form
        /// </summary>
        public string ShortForm { get; set; } = string.Empty;
    }
}
