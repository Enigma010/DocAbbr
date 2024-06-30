using System.Diagnostics.CodeAnalysis;

namespace AppCore
{
    /// <summary>
    /// Base entity that all entities derive from, unifies the definition that all
    /// entities will have an ID that is the primary identifier
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class Entity<IdType> : IEntity<IdType>
    {
        private List<object> _events = new List<object>();
        /// <summary>
        /// Creates a new entitty
        /// </summary>
        public Entity(Func<IdType> getNewId)
        {
            Id = getNewId();
        }
        /// <summary>
        /// The ID of the entity
        /// </summary>
        public IdType Id { get; protected set; }
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
            _events.Add(@event);
        }
        /// <summary>
        /// Gets all of the events
        /// </summary>
        /// <returns>The events</returns>
        public IReadOnlyCollection<object> GetEvents()
        {
            return _events.AsReadOnly();
        }
        /// <summary>
        /// Clears the events
        /// </summary>
        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
