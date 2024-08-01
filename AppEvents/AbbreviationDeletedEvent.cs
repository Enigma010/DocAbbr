using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvents
{
    /// <summary>
    /// Abbreviation deleted
    /// </summary>
    public class AbbreviationDeletedEvent
    {
        /// <summary>
        /// Create Abbreviation deleted event
        /// </summary>
        public AbbreviationDeletedEvent(string shortForm, 
            string longForm,
            List<string> description,
            List<Tuple<string, string>> links) 
        { 
            ShortForm = shortForm;
            LongForm = longForm;
            Description = description;
            links.ForEach(link => ReferenceLinks.Add(new Link(link.Item1, link.Item2)));
        }
        /// <summary>
        /// The short form
        /// </summary>
        public string ShortForm
        {
            get;
            private set;
        }
        /// <summary>
        /// The long form
        /// </summary>
        public string LongForm 
        { 
            get; 
            private set; 
        }
        /// <summary>
        /// The description
        /// </summary>
        public List<string> Description
        {
            get;
            private set;
        } = new List<string>();
        /// <summary>
        /// The reference links
        /// </summary>
        public List<Link> ReferenceLinks
        {
            get;
            private set;
        } = new List<Link>();
    }
}
