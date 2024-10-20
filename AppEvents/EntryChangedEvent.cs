using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvents
{
    /// <summary>
    /// Abbreviation changed event
    /// </summary>
    public class EntryChangedEvent
    {
        /// <summary>
        /// Create abbreviation changed event
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="oldDescription"></param>
        /// <param name="newDescription"></param>
        public EntryChangedEvent(
            string id,
            string name,
            IEnumerable<string> oldDescription,
            IEnumerable<string> newDescription) 
        {
            Id = id;
            Name = name;
            OldDescription = oldDescription.ToList();
            NewDescription = newDescription.ToList();
        }

        /// <summary>
        /// The ID
        /// </summary>
        public string Id 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The name of the entry
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        
        /// <summary>
        /// The old description
        /// </summary>
        public List<string> OldDescription { get; private set; } = new List<string>();
        
        /// <summary>
        /// The new description
        /// </summary>
        public List<string> NewDescription { get; private set; } = new List<string>();
    }
}
