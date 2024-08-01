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
        /// <param name="shortForm">The abbreviatino name</param>
        public AbbreviationCreatedEvent(
            string shortForm,
            string longForm,
            IEnumerable<string> description) 
        { 
            ShortForm = shortForm;
            LongForm = longForm;
            Description = description.ToList();
        }

        /// <summary>
        /// The abbreviatin name, for example CD
        /// for compact disk
        /// </summary>
        public string ShortForm { get; private set; } = string.Empty;

        /// <summary>
        /// The long form, for example compact disk
        /// </summary>
        public string LongForm { get; private set; } = string.Empty;

        /// <summary>
        /// The description
        /// </summary>
        public List<string> Description { get; private set; } = new List<string>();
    }
}
