using App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Commands
{
    /// <summary>
    /// Change abbreviation links command
    /// </summary>
    public class ChangeAbbreviationLinksCmd
    {
        /// <summary>
        /// Create a change abbreviation links command
        /// </summary>
        /// <param name="links"></param>
        public ChangeAbbreviationLinksCmd(List<Link> links) 
        { 
            Links = links;
        }
        /// <summary>
        /// The links
        /// </summary>
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
