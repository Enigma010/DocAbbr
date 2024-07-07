using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvents
{
    /// <summary>
    /// Abbreviation links changed event
    /// </summary>
    public class AbbreviationReferenceLinksChangedEvent
    {
        /// <summary>
        /// Create abbreviation links changed event
        /// </summary>
        /// <param name="addedReferenceLinks">The links added</param>
        /// <param name="changedReferenceLinks">The links changed</param>
        /// <param name="deletedReferenceLinks">The links deleted</param>
        public AbbreviationReferenceLinksChangedEvent(IEnumerable<Link> addedReferenceLinks, 
            IEnumerable<Link> changedReferenceLinks, 
            IEnumerable<Link> deletedReferenceLinks) 
        {
            AddedReferenceLinks = addedReferenceLinks.ToList();
            ChangedReferenceLinks = changedReferenceLinks.ToList();
            DeletedReferenceLinks = deletedReferenceLinks.ToList();
        }
        /// <summary>
        /// The links added
        /// </summary>
        public List<Link> AddedReferenceLinks { get; private set; } = new List<Link>();
        /// <summary>
        /// The links changed
        /// </summary>
        public List<Link> ChangedReferenceLinks { get; private set; } = new List<Link>();
        /// <summary>
        /// The links deleted
        /// </summary>
        public List<Link> DeletedReferenceLinks { get; private set; } = new List<Link>();
    }
}
