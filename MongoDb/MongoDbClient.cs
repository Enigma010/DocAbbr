using Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Logging;

namespace MongoDb
{
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class MongoDbClient : IDbClient
    {
        /// <summary>
        /// The default database name, if not specified in the configuration this value will be used
        /// </summary>
        public const string DefaultDatabase = "db";
        /// <summary>
        /// The URI where MongoDB is located
        /// </summary>
        private readonly string _uri = string.Empty;
        /// <summary>
        /// The database to use
        /// </summary>
        private readonly string _database = DefaultDatabase;
        /// <summary>
        /// The MongoDB client
        /// </summary>
        private readonly MongoClient _client;
        /// <summary>
        /// The client session
        /// </summary>
        private IClientSessionHandle? _session;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// Creates a new MongoDB client.
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <exception cref="NullReferenceException">Thrown if configuration is missing</exception>
        public MongoDbClient(IConfiguration configuration, ILogger<MongoDbClient> logger)
        {
            _uri = configuration.GetSection("Db")["Uri"] ?? throw new NullReferenceException("Missing Db:Uri in the configuration");
            string username = configuration.GetSection("Db")["Username"] ?? throw new NullReferenceException("Missing Db.Username in the configuration");
            string password = configuration.GetSection("Db")["Password"] ?? throw new NullReferenceException("Missing Db.Password in the configuration");
            _uri = _uri.Replace(IDbClient.UserNamePattern, Uri.EscapeDataString(username));
            _uri = _uri.Replace(IDbClient.PasswordPattern, Uri.EscapeDataString(password));
            _database = configuration.GetSection("Db")["Database"] ?? throw new NullReferenceException("Missing Db:DatabaseName in the configuration");
            _client = new MongoClient(_uri);
            _logger = logger;
        }
        /// <summary>
        /// Inserts an entity into MongoDB
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        /// <typeparam name="IdType"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertAsync<EntityType, IdType>(EntityType entity) where EntityType : IDbEntity<IdType>
        {
            using (_logger.LogCaller())
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Null entities cannot be saved");
                }
                if (_session == null)
                {
                    throw new InvalidOperationException("Begin transatino must be called first");
                }
                var collection = GetCollectionForEntityType<EntityType>();
                _logger.LogInformation("Inserting {Id}", entity.Id);
                await collection.InsertOneAsync(_session, entity);
                _logger.LogInformation("Inserted {Id}", entity.Id);
            }
        }

        /// <summary>
        /// Updates an entity
        /// </summary>
        /// <typeparam name="EntityType">The type of entity</typeparam>
        /// <typeparam name="IdType">The type of the entities ID</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if the entity is null</exception>
        public async Task UpdateAsync<EntityType, IdType>(EntityType entity) where EntityType : IDbEntity<IdType> where IdType : IComparable
        {
            using (_logger.LogCaller())
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Null entities cannot be saved");
                }
                var collection = GetCollectionForEntityType<EntityType>();
                var filter = IdFilter<EntityType, IdType>(entity.Id);
                _logger.LogInformation("Replacing {Id}", entity.Id);
                await collection.ReplaceOneAsync<EntityType>(_session, doc => doc.Id.Equals(entity.Id), entity);
                _logger.LogInformation("Replaced {Id}", entity.Id);
            }
        }

        /// <summary>
        /// Deletes and entity
        /// </summary>
        /// <typeparam name="EntityType">The entity to delete</typeparam>
        /// <typeparam name="IdType">The ID of the entity</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns></returns>
        public async Task DeleteAsync<EntityType, IdType>(EntityType entity) where EntityType : IDbEntity<IdType>
        {
            using (_logger.LogCaller())
            {
                if (_session == null)
                {
                    throw new InvalidOperationException("Begin transatino must be called first");
                }
                var collection = GetCollectionForEntityType<EntityType>();
                var filter = IdFilter<EntityType, IdType>(entity.Id);
                _logger.LogInformation("Deleting {Id}", entity.Id);
                await collection.DeleteOneAsync(_session, filter);
                _logger.LogInformation("Deleted {Id}", entity.Id);
            }
        }

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        /// <typeparam name="EntityType">The type of entity</typeparam>
        /// <typeparam name="IdType">The type of the ID</typeparam>
        /// <param name="id">The ID of the entity</param>
        /// <returns>The entity</returns>
        /// <exception cref="DbEntityNotFoundException{IdType}">Thrown if the entity isn't found</exception>
        public async Task<EntityType> GetAsync<EntityType, IdType>(IdType id)
        {
            using (_logger.LogCaller())
            {
                var collection = GetCollectionForEntityType<EntityType>();
                var filter = IdFilter<EntityType, IdType>(id);
                _logger.LogInformation("Getting {Id}", id);
                var entities = await (await collection.FindAsync<EntityType>(filter)).ToListAsync();
                _logger.LogInformation("Got {Id}", id);
                if (!entities.Any())
                {
                    throw new DbEntityNotFoundException<IdType>(id);
                }
                return entities.First();
            }
        }

        /// <summary>
        /// Gets and entity based on an expression
        /// </summary>
        /// <typeparam name="EntityType">The entity type</typeparam>
        /// <param name="expression">The expression to get the entity by</param>
        /// <returns></returns>
        public async Task<IEnumerable<EntityType>> GetAsync<EntityType>(Expression<Func<EntityType, bool>> expression)
        {
            using (_logger.LogCaller())
            {
                var collection = GetCollectionForEntityType<EntityType>();
                var filter = Builders<EntityType>.Filter.Where(expression);
                _logger.LogInformation("Getting by expression");
                var entities = await (await collection.FindAsync<EntityType>(filter)).ToListAsync();
                _logger.LogInformation("Got by expression");
                return entities;
            }
        }

        /// <summary>
        /// Gets all the entities
        /// </summary>
        /// <typeparam name="EntityType">The type of entity</typeparam>
        /// <typeparam name="IdType">The type of ID</typeparam>
        /// <returns>The entities</returns>
        public async Task<IEnumerable<EntityType>> GetAsync<EntityType, IdType>()
        {
            using (_logger.LogCaller())
            {
                var collection = GetCollectionForEntityType<EntityType>();
                var filter = Builders<EntityType>.Filter.Empty;
                _logger.LogInformation("Getting all");
                var entities = await (await collection.FindAsync<EntityType>(filter)).ToListAsync();
                _logger.LogInformation("Got all");
                return entities;
            }
        }

        /// <summary>
        /// Begins a unit of work
        /// </summary>
        /// <returns></returns>
        public async Task Begin()
        {
            if (_session == null)
            {
                _session = await _client.StartSessionAsync();
            }
            _session.StartTransaction();
        }

        /// <summary>
        /// Ends a unit of work
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if begin unit of work wasn't called</exception>
        public async Task Commit()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("Commit cannot be called without a call to Begin");
            }
            if (_session.IsInTransaction)
            {
                await _session.CommitTransactionAsync();
            }
        }

        /// <summary>
        /// Rolls back a unit of work
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Rollback()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("Rollback cannot be called without a call to Begin");
            }
            if(_session.IsInTransaction)
            {
                _session.AbortTransaction();
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Resolves a C# class to a MongoDB collection
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        /// <returns></returns>
        private IMongoCollection<EntityType> GetCollection<EntityType>()
        {
            return _client.GetDatabase(_database).GetCollection<EntityType>(typeof(EntityType).Name);
        }

        /// <summary>
        /// Gets a filter clause base on the ID
        /// </summary>
        /// <typeparam name="EntityType">The type of entity</typeparam>
        /// <typeparam name="IdType">The type of ID</typeparam>
        /// <param name="id">The ID</param>
        /// <returns></returns>
        private FilterDefinition<EntityType> IdFilter<EntityType, IdType>(IdType id)
        {
            return Builders<EntityType>.Filter.Eq("_id", id);
        }
        /// <summary>
        /// Logs the message before we get the collection name
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        private IMongoCollection<EntityType> GetCollectionForEntityType<EntityType>()
        {
            _logger.LogInformation("Getting collection for {EntityName}", typeof(EntityType).Name);
            IMongoCollection<EntityType> collection = GetCollection<EntityType>();
            _logger.LogInformation("Got collection for {EntityName} is {CollectionName}", typeof(EntityType).Name, collection.CollectionNamespace.CollectionName);
            return collection;
        }
    }
}
