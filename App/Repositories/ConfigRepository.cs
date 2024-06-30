using App.Entities;
using AppCore.Repositories;
using Db;
using Microsoft.Extensions.Logging;

namespace App.Repositories
{
    /// <summary>
    /// Configuration repository saves configuration options to the data store
    /// </summary>
    public interface IConfigRepository : IBaseRepository<Config, Guid>
    {
    }
    /// <summary>
    /// Configuration object for saving data to the data store
    /// </summary>
    public class ConfigRepository : BaseRepository<IConfigRepository, Config, Guid>, IConfigRepository
    {
        public ConfigRepository(IDbClient client, ILogger<IConfigRepository> logger)
            : base(client, logger)
        {
        }
    }
}
