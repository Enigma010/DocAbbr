using App.IEntities;
using App.Repositories.Dtos;
using AppCore;

namespace App.Entities
{
    /// <summary>
    /// A link
    /// </summary>
    public class Link : Entity<LinkDto, string>, IReadableLink
    {
        /// <summary>
        /// Creates a link
        /// </summary>
        /// <param name="link"></param>
        public Link(LinkDto link) :base(link)
        {
        }

        /// <summary>
        /// Creates an empty link
        /// </summary>
        public Link() : base(() => { return string.Empty; })
        {
        }

        /// <summary>
        /// Creates a new link
        /// </summary>
        /// <param name="getNewId">The method to generate a new ID</param>
        /// <param name="url">The URL</param>
        public Link(string url, string linkText) : base(() => { return url; })
        {
            _dto.Url = url;
            _dto.LinkText = linkText;
        }

        /// <summary>
        /// The URL of the link
        /// </summary>
        public string Url => _dto.Url;

        /// <summary>
        /// 
        /// </summary>
        public string LinkText => _dto.LinkText;
    }
}
