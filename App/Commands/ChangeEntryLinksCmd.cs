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
    /// Change entry links command
    /// </summary>
    public class ChangeEntryLinksCmd
    {
        /// <summary>
        /// Create a change entry links command
        /// </summary>
        public ChangeEntryLinksCmd() { }

        /// <summary>
        /// Creates a change entry links command
        /// </summary>
        /// <param name="links"></param>
        public ChangeEntryLinksCmd(IEnumerable<IReadableLink> links)
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
