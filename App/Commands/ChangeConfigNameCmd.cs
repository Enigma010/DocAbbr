using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Commands
{
    /// <summary>
    /// Represents a set of changes that can be done to a configuration
    /// </summary>
    public class ChangeConfigNameCmd
    {
        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
