using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvents
{
    public class Link
    {
        /// <summary>
        /// Creates a new link
        /// </summary>
        /// <param name="getNewId">The method to generate a new ID</param>
        /// <param name="url">The URL</param>
        public Link(string url, string linkText)
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
    }
}
