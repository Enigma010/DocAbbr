using App.Entities;
using App.Repositories.Dtos;
using AppCore.Repositories;
using Db;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
    /// <summary>
    /// Entry repository saves entry to the data store
    /// </summary>
    public interface IEntryRepository : IBaseRepository<Entry, EntryDto, string>
    {
    }
    /// <summary>
    /// Entry repository saves entry to the data store
    /// </summary>
    public class EntryRepository : BaseRepository<IEntryRepository, Entry, EntryDto, string>, IEntryRepository
    {
        public EntryRepository(IDbClient client, ILogger<IEntryRepository> logger) : base(client, logger)
        {
        }
    }
}
