using App.Entities;
using App.IEntities;
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
        public ChangeAbbreviationLinksCmd() { }

        /// <summary>
        /// Creates a change abbreviatino links command
        /// </summary>
        /// <param name="links"></param>
        public ChangeAbbreviationLinksCmd(IEnumerable<IReadableLink> links)
        {
            links.ToList().ForEach(link => Links.Add(new ChangeLinkCmd() 
            { 
                LinkText = link.LinkText, 
                Url = link.Url 
            }));
        }

        /// <summary>
        /// The links
        /// </summary>
        public List<ChangeLinkCmd> Links { get; set; } = new List<ChangeLinkCmd>();
    }
}
