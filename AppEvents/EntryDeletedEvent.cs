using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvents
{
    /// <summary>
    /// Entry deleted
    /// </summary>
    public class EntryDeletedEvent
    {
        /// <summary>
        /// Create entry deleted event
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="name">The name of the entry</param>
        /// <param name="description">The description of the entry</param>
        public EntryDeletedEvent(
            string id,
            string name, 
            List<string> description) 
        {
            Id = id;
            Name = name;
            Description = description;
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
        /// The short form
        /// </summary>
        public string Name
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
    }
}
