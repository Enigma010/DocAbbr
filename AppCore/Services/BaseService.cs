using AppCore.Repositories;
using EventBus;
using System.Diagnostics.CodeAnalysis;
using UnitOfWork;
using Logging;
using Microsoft.Extensions.Logging;

namespace AppCore.Services
{
    /// <summary>
    /// The base service interface, the service interface deals with methods to interact with
    /// repository and other services and returns you entities to work with to define your
    /// application logic
    /// </summary>
    /// <typeparam name="EntityType">The entity type</typeparam>
    /// <typeparam name="IdType">The ID type that identifies the entity</typeparam>
    public interface IBaseService<EntityType, IdType>
    {
        Task<EntityType> ChangeAsync(IdType id, Func<EntityType, EntityType> changeFunc);
    }
    /// <summary>
    /// The base service object
    /// </summary>
    /// <typeparam name="RepositoryType">The repository type</typeparam>
    /// <typeparam name="EntityType">The entity type</typeparam>
    /// <typeparam name="IdType">The ID type</typeparam>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class BaseService<RepositoryType, EntityType, EntityDtoType, IdType> 
        : IBaseService<EntityType, IdType> 
        where RepositoryType : IBaseRepository<EntityType, EntityDtoType, IdType>
        where EntityType : IEntity<EntityDtoType, IdType>
        where EntityDtoType : EntityDto<IdType>
        where IdType : IComparable
    {
        /// <summary>
        /// The repository
        /// </summary>
        protected readonly RepositoryType _repository;
        /// <summary>
        /// The event bus publisher
        /// </summary>
        protected readonly IEventPublisher _eventPublisher;
        /// <summary>
        /// List of unit of work objects
        /// </summary>
        protected readonly List<IUnitOfWork> _unitOfWorks = new List<IUnitOfWork>();
        /// <summary>
        /// Logger object
        /// </summary>
        protected readonly ILogger _logger;
        /// <summary>
        /// Crates a new base service
        /// </summary>
        /// <param name="repository">The repository</param>
        public BaseService(RepositoryType repository, IEventPublisher eventPublisher, ILogger logger)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _logger = logger;
            _unitOfWorks.AddRange(new List<IUnitOfWork>() { _repository, _eventPublisher });
        }
        /// <summary>
        /// Handles standard pattern of change/update, standard pattern is load the eneity from
        /// the repository, call a method that updates the entity in process, then call the
        /// repository method to update it in the repository
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <param name="changeFunc">The function to change the entity</param>
        /// <returns>The changed entity</returns>
        public async Task<EntityType> ChangeAsync(IdType id, Func<EntityType, EntityType> changeFunc)
        {
            using (_logger.LogCaller())
            {
                using (var unitOfWorks = new UnitOfWorks(_unitOfWorks, _logger))
                {
                    EntityType entity = await _repository.GetAsync(id);
                    _logger.LogInformation("Running change function on {Id}", id);
                    changeFunc(entity);
                    _logger.LogInformation("Ran change function on {Id}", id);
                    entity = await _repository.UpdateAsync(entity);
                    await unitOfWorks.Commit();
                    return entity;
                }
            }
        }
        /// <summary>
        /// Publishes state changes as events on the event bus
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns></returns>
        protected async Task PublishEvents(EntityType entity)
        {
            using (_logger.LogCaller())
            {
                await _eventPublisher.Publish(entity.GetEvents());
            }
        }
    }
}
