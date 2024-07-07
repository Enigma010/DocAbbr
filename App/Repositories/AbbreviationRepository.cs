using App.Entities;
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
    /// Abbreviation repository saves abbreviation to the data store
    /// </summary>
    public interface IAbbreviationRepository : IBaseRepository<Abbreviation, string>
    {
    }
    /// <summary>
    /// Abbreviation repository saves abbreviation to the data store
    /// </summary>
    public class AbbreviationRepository : BaseRepository<IAbbreviationRepository, Abbreviation, string>, IAbbreviationRepository
    {
        public AbbreviationRepository(IDbClient client, ILogger<IAbbreviationRepository> logger) : base(client, logger)
        {
        }
    }
}
