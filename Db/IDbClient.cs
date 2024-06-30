using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using UnitOfWork;

namespace Db
{
    /// <summary>
    /// Interface to a database, supports CRUD operations, used so that different database can be
    /// substituted without impacting the repository service
    /// </summary>
    public interface IDbClient : IUnitOfWork
    {
        public const string UserNamePattern = "{username}";
        public const string PasswordPattern = "{password}";
        public const string PortPattern = "{port}";
        Task InsertAsync<EntityType, IdType>(EntityType entity) where EntityType : IDbEntity<IdType>;
        Task DeleteAsync<EntityType, IdType>(EntityType entity) where EntityType : IDbEntity<IdType>;
        Task UpdateAsync<EntityType, IdType>(EntityType entity) where EntityType : IDbEntity<IdType> where IdType : IComparable;
        Task<EntityType> GetAsync<EntityType, IdType>(IdType id);
        Task<IEnumerable<EntityType>> GetAsync<EntityType, IdType>();
        Task<IEnumerable<EntityType>> GetAsync<EntityType>(Expression<Func<EntityType, bool>> expression);
    }
}
