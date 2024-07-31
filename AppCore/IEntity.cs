using Db;

namespace AppCore
{
    /// <summary>
    /// Entity interface, allows database to be able to find the ID
    /// of the entity
    /// </summary>
    /// <typeparam name="IdType">The type of ID used to identify the entity</typeparam>
    public interface IEntity<EntityDtoType, IdType> : IDbEntity<IdType> 
        where EntityDtoType : EntityDto<IdType>
    {
        public void Deleted();
        public void AddEvent(object addEvent);
        public IReadOnlyCollection<object> GetEvents();
        public void ClearEvents();
        public EntityDtoType GetDto();
    }
}
