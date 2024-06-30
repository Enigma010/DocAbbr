using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore
{
    /// <summary>
    /// The application configurations
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class AppConfig
    {
        public const string ConfigurationSectionName = "App";
        public const string NameSectionName = "Name";
        /// <summary>
        /// Creates a new app configuration object
        /// </summary>
        /// <param name="configurationManager"></param>
        public AppConfig(IConfigurationManager configurationManager) 
        {
            IConfigurationSection section = configurationManager.GetSection(ConfigurationSectionName);
            Name = section[NameSectionName] ?? string.Empty;
        }
        /// <summary>
        /// The name of the application
        /// </summary>
        public string Name { get; private set; } = string.Empty;
    }
}
