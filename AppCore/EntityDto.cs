using Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore
{
    public class EntityDto<IdType> : IDbEntity<IdType>
    {
        /// <summary>
        /// Creates a new entity data transfer object
        /// </summary>
        /// <param name="getNewId"></param>
        public EntityDto(Func<IdType> getNewId)
        {
            Id = getNewId();
        }
        /// <summary>
        /// The ID of the entity
        /// </summary>
        public IdType Id { get; set; }
    }
}
