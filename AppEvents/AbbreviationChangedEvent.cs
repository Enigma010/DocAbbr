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
    public class AbbreviationChangedEvent
    {
        /// <summary>
        /// Create abbreviation changed event
        /// </summary>
        /// <param name="oldFullName"></param>
        /// <param name="newFullName"></param>
        /// <param name="oldDescription"></param>
        /// <param name="newDescription"></param>
        public AbbreviationChangedEvent(string oldFullName, 
            string newFullName,
            string oldDescription,
            string newDescription) 
        { 
            OldFullName = oldFullName;
            NewFullName = newFullName;
            OldDescription = oldDescription;
            NewDescription = newDescription;
        }
        /// <summary>
        /// The new full name
        /// </summary>
        public string NewFullName { get; private set; } = string.Empty;
        /// <summary>
        /// The old full name
        /// </summary>
        public string OldFullName { get; private set; } = string.Empty;
        /// <summary>
        /// The old description
        /// </summary>
        public string OldDescription { get; private set; } = string.Empty;
        /// <summary>
        /// The new description
        /// </summary>
        public string NewDescription { get; private set; } = string.Empty;
    }
}
