using Db;

namespace AppCore
{
    /// <summary>
    /// Entity interface, allows database to be able to find the ID
    /// of the entity
    /// </summary>
    /// <typeparam name="IdType">The type of ID used to identify the entity</typeparam>
    public interface IEntity<IdType> : IDbEntity<IdType>
    {
        public void Deleted();
        public void AddEvent(object stateChanged);
        public IReadOnlyCollection<object> GetEvents();
        public void ClearEvents();
    }
}
