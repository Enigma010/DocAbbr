using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvents
{
    public class AbbreviationCreatedEvent
    {
        /// <summary>
        /// Abbreviation created
        /// </summary>
        /// <param name="name">The abbreviatino name</param>
        public AbbreviationCreatedEvent(string name) 
        { 
            Name = name;
        }
        /// <summary>
        /// The abbreviatin name, for example CD
        /// for compact disk
        /// </summary>
        public string Name { get; private set; } = string.Empty;
    }
}
