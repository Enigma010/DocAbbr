using AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities
{
    /// <summary>
    /// A link
    /// </summary>
    public class Link : Entity<string>
    {
        /// <summary>
        /// Creates a new link
        /// </summary>
        /// <param name="getNewId">The method to generate a new ID</param>
        /// <param name="url">The URL</param>
        public Link(string url, string linkText) : base(() => { return url; })
        {
            Url = url;
            LinkText = linkText;
        }

        /// <summary>
        /// The URL of the link
        /// </summary>
        public string Url
        {
            get;
            private set;
        } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string LinkText
        {
            get;
            private set;
        } = string.Empty;
        /// <summary>
        /// Gets the links that were added
        /// </summary>
        /// <param name="links">The links</param>
        /// <param name="newLinks">The new links</param>
        /// <returns>The links added</returns>
        public static IEnumerable<Link> Added(IEnumerable<Link> links, IEnumerable<Link> newLinks)
        {
            List<Link> added = new List<Link>();
            foreach (var newLink in newLinks)
            {
                if(!links.Any((checkLink) => checkLink.Id == newLink.Id))
                {
                    added.Add(newLink);
                }
            }
            return added;
        }
        /// <summary>
        /// Gets the links that were deleted
        /// </summary>
        /// <param name="links">The links</param>
        /// <param name="newLinks">The new links</param>
        /// <returns>The links deleted</returns>
        public static IEnumerable<Link> Deleted(IEnumerable<Link> links, IEnumerable<Link> newLinks)
        {
            List<Link> deleted = new List<Link>();
            foreach (var link in links)
            {
                if (!newLinks.Any((checkLink) => checkLink.Id == link.Id))
                {
                    deleted.Add(link);
                }
            }
            return deleted;
        }
        /// <summary>
        /// Gets the links that were changed
        /// </summary>
        /// <param name="links">The links</param>
        /// <param name="newLinks">The new links</param>
        /// <returns>The links that were deleted</returns>
        public static IEnumerable<Link> Changed(IEnumerable<Link> links, IEnumerable<Link> newLinks)
        {
            List<Link> changed = new List<Link>();
            foreach (var newLink in newLinks)
            {
                Link? existingLink = links.FirstOrDefault((checkLink) => checkLink.Id == newLink.Id);
                if (existingLink != null && existingLink.LinkText != newLink.LinkText)
                {
                    changed.Add(newLink);
                }
            }
            return changed;
        }
    }
}
