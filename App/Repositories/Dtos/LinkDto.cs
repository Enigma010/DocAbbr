using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using App.Commands;
using App.IEntities;
using AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Dtos
{
    public class LinkDto : EntityDto<string>, IReadableLink
    {
        private string _url = string.Empty;
        /// <summary>
        /// Creates a new link
        /// </summary>
        /// <param name="getNewId"></param>
        public LinkDto(Func<string> getNewId) : base(getNewId)
        {
            Url = Id;
        }

        /// <summary>
        /// Creates a new link
        /// </summary>
        /// <param name="url"></param>
        /// <param name="linkText"></param>
        public LinkDto(string url, string linkText) : base(() => { return url; })
        {
            LinkText = linkText;
        }

        /// <summary>
        /// The URL of the link
        /// </summary>
        public string Url
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
            }
        }

        /// <summary>
        /// The link text
        /// </summary>
        public string LinkText
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// Gets the links that were added
        /// </summary>
        /// <param name="links">The links</param>
        /// <param name="newLinks">The new links</param>
        /// <returns>The links added</returns>
        public static IEnumerable<LinkDto> Added(IEnumerable<LinkDto> links, IEnumerable<ChangeLinkCmd> newLinks)
        {
            List<LinkDto> added = new List<LinkDto>();
            foreach (var newLink in newLinks)
            {
                if (!links.Any((checkLink) => checkLink.Id == newLink.Url))
                {
                    added.Add(new LinkDto(newLink.Url, newLink.LinkText));
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
        public static IEnumerable<LinkDto> Deleted(IEnumerable<LinkDto> links, IEnumerable<ChangeLinkCmd> newLinks)
        {
            List<LinkDto> deleted = new List<LinkDto>();
            foreach (var link in links)
            {
                if (!newLinks.Any((checkLink) => checkLink.Url == link.Id))
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
        public static IEnumerable<LinkDto> Changed(IEnumerable<LinkDto> links, IEnumerable<ChangeLinkCmd> newLinks)
        {
            List<LinkDto> changed = new List<LinkDto>();
            foreach (var newLink in newLinks)
            {
                LinkDto? existingLink = links.FirstOrDefault((checkLink) => checkLink.Id == newLink.Url);
                if (existingLink != null && existingLink.LinkText != newLink.LinkText)
                {
                    changed.Add(new LinkDto(newLink.Url, newLink.LinkText));
                }
            }
            return changed;
        }
    }
}
