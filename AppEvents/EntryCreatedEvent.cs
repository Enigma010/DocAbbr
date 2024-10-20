using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvents
{
    public class EntryCreatedEvent
    {
        /// <summary>
        /// Abbreviation created
        /// </summary>
        /// <param name="name">The entry name</param>
        public EntryCreatedEvent(
            string id,
            string name,
            IEnumerable<string> description) 
        {
            Id = id;
            Name = name;
            Description = description.ToList();
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
        /// The entry name, for example CD
        /// for compact disk
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// The description
        /// </summary>
        public List<string> Description { get; private set; } = new List<string>();
    }
}
