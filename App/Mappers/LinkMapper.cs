using AppEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Mappers
{
    public static class LinkMapper
    {
        /// <summary>
        /// Maps a set of link eneities to app event links
        /// </summary>
        /// <param name="links">The link entities</param>
        /// <returns></returns>
        public static IEnumerable<AppEvents.Link> Map(this IEnumerable<Entities.Link> links)
        {
            return links.Select((lnk) => Map(lnk));
        }
        /// <summary>
        /// Maps a link entity to app event link
        /// </summary>
        /// <param name="link">The link entity</param>
        /// <returns></returns>
        public static AppEvents.Link Map(this Entities.Link link)
        {
            return new AppEvents.Link(link.Url, link.LinkText);
        }
    }
}
