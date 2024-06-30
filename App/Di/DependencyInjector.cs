using App.Repositories;
using App.Services;
using AppCore;
using EventBus.Di;
using Logging.Di;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDb.Di;
using System.Diagnostics.CodeAnalysis;

namespace App.Di
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjector
    {
        /// <summary>
        /// Registers dependencies for the application
        /// </summary>
        /// <param name="builder">The application host builder</param>
        public static void AddAppDependencies(this IHostApplicationBuilder builder)
        {
            builder.AddMongoDbDependencies();
            AppConfig appConfig = new AppConfig(builder.Configuration);
            builder.AddEventBusDependencies(appConfig.Name, ["AppEvents"], ["AppEventConsumers"]);
            builder.AddLoggerDependencies([
                new Logging.Di.DependencyInjector.JsonFileConfig("appsettings.json", optional: false),
                new Logging.Di.DependencyInjector.JsonFileConfig("appsettings.Development.json", optional: true),
                ]);
            builder.Services.AddScoped<IConfigService, ConfigService>();
            builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
        }
    }
}
