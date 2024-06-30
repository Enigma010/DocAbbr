﻿using Db;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using UnitOfWork;
using Logging;

namespace AppCore.Repositories
{
    /// <summary>
    /// Base repository interface
    /// </summary>
    /// <typeparam name="EntityType">The entity type</typeparam>
    /// <typeparam name="IdType">The ID type of that entity</typeparam>
    public interface IBaseRepository<EntityType, IdType> : IUnitOfWork
    {
        Task<EntityType> GetAsync(IdType id);
        Task<IEnumerable<EntityType>> GetAsync();
        Task<IEnumerable<EntityType>> GetAsync(Expression<Func<EntityType, bool>> expression);
        Task<EntityType> InsertAsync(EntityType entity);
        Task<EntityType> UpdateAsync(EntityType entity);
        Task DeleteAsync(EntityType entity);
    }
    /// <summary>
    /// Base repository object, the repository object interfaces with a data storage
    /// service, like a database.  This base repository supports standarnd operations like get entity,
    /// get all entities, create entity, update endity, delete entity.  Typically all you need to
    /// do is extend this class and that's all you need to do to support standard CRUD operations
    /// </summary>
    /// <typeparam name="RepositoryType">The type of repository</typeparam>
    /// <typeparam name="EntityType">The type of eneity</typeparam>
    /// <typeparam name="IdType">The type of ID</typeparam>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class BaseRepository<RepositoryType, EntityType, IdType> 
        : IBaseRepository<EntityType, IdType> where EntityType 
        : IEntity<IdType> where IdType 
        : IComparable
    {
        protected readonly ILogger<RepositoryType> _logger;
        protected readonly IDbClient _client;
        public BaseRepository(IDbClient client,
            ILogger<RepositoryType> logger)
        {
            _client = client;
            _logger = logger;
        }
        /// <summary>
        /// Insert or createa a new entity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The entity created</returns>
        public virtual async Task<EntityType> InsertAsync(EntityType entity) 
        {
            using (_logger.LogCaller())
            {
                _logger.LogInformation("Inserting {Id}", entity.Id);
                await _client.InsertAsync<EntityType, IdType>(entity);
                _logger.LogInformation("Inserted {Id}", entity.Id);
                return entity;
            }
        }

        /// <summary>
        /// Updates an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The entity updated</returns>
        public virtual async Task<EntityType> UpdateAsync(EntityType entity) 
        {
            using (_logger.LogCaller())
            {
                _logger.LogInformation("Updating {Id}", entity.Id);
                await _client.UpdateAsync<EntityType, IdType>(entity);
                _logger.LogInformation("Updated {Id}", entity.Id);
                return entity;
            }
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(EntityType entity)
        {
            using (_logger.LogCaller())
            {
                _logger.LogInformation("Marking entity {Id} as deleted", entity.Id);
                entity.Deleted();
                _logger.LogInformation("Marked entity {Id} as deleted", entity.Id);
                _logger.LogInformation("Deleting {Id}", entity.Id);
                await _client.DeleteAsync<EntityType, IdType>(entity);
                _logger.LogInformation("Deleted {Id}", entity.Id);
            }
        }

        /// <summary>
        /// Gets an entity identified by the ID
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <returns>The entity</returns>
        public virtual async Task<EntityType> GetAsync(IdType id)
        {
            using (_logger.LogCaller())
            {
                _logger.LogInformation("Getting {Id}", id);
                EntityType entity = await _client.GetAsync<EntityType, IdType>(id);
                _logger.LogInformation("Got {Id}", id);
                ClearEvents(new List<EntityType> { entity });
                return entity;
            }
        }

        /// <summary>
        /// Gets entities based on an expression
        /// </summary>
        /// <typeparam name="EntityType">The entity type</typeparam>
        /// <param name="expression">The filter expression</param>
        /// <returns>The entities that match the expression</returns>
        public virtual async Task<IEnumerable<EntityType>> GetAsync(Expression<Func<EntityType, bool>> expression)
        {
            using (_logger.LogCaller())
            {
                _logger.LogInformation("Getting by expression");
                IEnumerable<EntityType> entities = await _client.GetAsync(expression);
                _logger.LogInformation("Got by expression");
                ClearEvents(entities);
                return entities;
            }
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>All of the entities</returns>
        public virtual async Task<IEnumerable<EntityType>> GetAsync()
        {
            using (_logger.LogCaller())
            {
                _logger.LogInformation("Getting all");
                IEnumerable<EntityType> entities = await _client.GetAsync<EntityType, IdType>();
                _logger.LogInformation("Got all");
                ClearEvents(entities);
                return entities;
            }
        }
        /// <summary>
        /// Clears the events on the entities
        /// </summary>
        /// <param name="entities">The entities</param>
        private void ClearEvents(IEnumerable<EntityType> entities)
        {
            using (_logger.LogCaller())
            {
                _logger.LogInformation("Clearing events");
                entities.ToList().ForEach(entity => entity.ClearEvents());
            }
        }

        /// <summary>
        /// Begins a unit of work
        /// </summary>
        /// <returns></returns>
        public async Task Begin()
        {
            await _client.Begin();
        }

        /// <summary>
        /// Commits a unit of work
        /// </summary>
        /// <returns></returns>
        public async Task Commit()
        {
            await _client.Commit();
        }
        /// <summary>
        /// Rollbacks a unit of work
        /// </summary>
        /// <returns></returns>
        public async Task Rollback()
        {
            await _client.Rollback();
        }
    }
}