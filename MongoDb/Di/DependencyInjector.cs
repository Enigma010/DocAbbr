using Db;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace MongoDb.Di
{
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would be in a different project")]
    public static class DependencyInjector
    {
        /// <summary>
        /// Does dependency injection for MongoDB
        /// </summary>
        /// <param name="builder">The host application builder</param>
        public static void AddMongoDbDependencies(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<IDbClient, MongoDbClient>();
        }
    }
}
