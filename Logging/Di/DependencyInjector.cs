using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Runtime.InteropServices;

namespace Logging.Di
{
    public static class DependencyInjector
    {
        public class JsonFileConfig
        {
            public JsonFileConfig(string fileName, bool optional = true)
            {
                FileName = fileName;
                Optional = optional;
            }
            public string FileName { get; set; } = string.Empty;
            public bool Optional { get; set; } = true;
            public bool ReloadOnChange { get; set; } = true;
        }
        public static void AddLoggerDependencies(this IHostApplicationBuilder builder, IEnumerable<JsonFileConfig> jsonFileConfigs)
        {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory());
                foreach(var jsonFileConfig in jsonFileConfigs)
                {
                    configuration = configuration.AddJsonFile(
                        jsonFileConfig.FileName, 
                        optional: jsonFileConfig.Optional, 
                        reloadOnChange: jsonFileConfig.ReloadOnChange);
                }
                var configurationBuild = configuration.Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationBuild)
                .CreateLogger();
            builder.Services.AddSerilog();
            builder.Services.AddSingleton(Log.Logger);
        }
        public static void AddWebLoggingDependencies(this WebApplication app)
        {
            app.UseSerilogRequestLogging();
        }
    }
}
