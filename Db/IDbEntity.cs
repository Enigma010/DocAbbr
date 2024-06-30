using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db
{
    /// <summary>
    /// Entity interface, allows database to be able to find the ID
    /// of the entity
    /// </summary>
    /// <typeparam name="IdType">The type of ID used to identify the entity</typeparam>
    public interface IDbEntity<IdType>
    {
        public IdType Id { get; }
    }
}
