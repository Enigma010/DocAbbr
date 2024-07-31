using App.Entities;
using App.Repositories.Dtos;
using AppCore.Repositories;
using Db;
using Microsoft.Extensions.Logging;

namespace App.Repositories
{
    /// <summary>
    /// Configuration repository saves configuration options to the data store
    /// </summary>
    public interface IConfigRepository : IBaseRepository<Config, ConfigDto, Guid>
    {
    }
    /// <summary>
    /// Configuration object for saving data to the data store
    /// </summary>
    public class ConfigRepository : BaseRepository<IConfigRepository, Config, ConfigDto, Guid>, IConfigRepository
    {
        public ConfigRepository(IDbClient client, ILogger<IConfigRepository> logger)
            : base(client, logger)
        {
        }
    }
}
