using Db;
using System.Diagnostics.CodeAnalysis;

namespace AppCore
{
    /// <summary>
    /// Base entity that all entities derive from, unifies the definition that all
    /// entities will have an ID that is the primary identifier
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class Entity<EntityDtoType, IdType> : IEntity<EntityDtoType, IdType>
        where EntityDtoType : EntityDto<IdType>
    {
        /// <summary>
        /// The events
        /// </summary>
        private List<object> _events = new List<object>();

        /// <summary>
        /// The data transport object
        /// </summary>
        protected readonly EntityDtoType _dto;
        /// <summary>
        /// Gets the ID
        /// </summary>
        public IdType Id => _dto.Id;

        /// <summary>
        /// Gets the data transfer objects
        /// </summary>
        /// <returns></returns>
        public EntityDtoType GetDto()
        {
            return _dto;
        }
        /// <summary>
        /// Creates a new entitty
        /// </summary>
        public Entity(EntityDtoType dto)
        {
            _dto = dto;
        }

        /// <summary>
        /// Creates a new entitty
        /// </summary>
        public Entity(Func<IdType> getNewId)
        {
            _dto = (EntityDtoType?)Activator.CreateInstance(typeof(EntityDtoType), getNewId) ?? throw new NullReferenceException();
        }

        /// <summary>
        /// Sets the entity state to deleted
        /// </summary>
        public virtual void Deleted()
        {
            throw new InvalidOperationException("Deleted method must be overridden");
        }
        /// <summary>
        /// Adds an event
        /// </summary>
        /// <param name="event">The event</param>
        public void AddEvent(object @event)
        {
            _events = _events ?? new List<object>();
            _events.Add(@event);
        }
        /// <summary>
        /// Gets all of the events
        /// </summary>
        /// <returns>The events</returns>
        public IReadOnlyCollection<object> GetEvents()
        {
            _events = _events ?? new List<object>();
            return _events.AsReadOnly();
        }
        /// <summary>
        /// Clears the events
        /// </summary>
        public void ClearEvents()
        {
            _events = _events ?? new List<object>();
            _events.Clear();
        }
    }
}
