namespace Db
{
    /// <summary>
    /// Exception thrown if the entity cannot be found in the database
    /// </summary>
    /// <typeparam name="IdType">The ID type of the entity</typeparam>
    public class DbEntityNotFoundException<IdType> : Exception
    {
        /// <summary>
        /// Creates a new exception
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        public DbEntityNotFoundException(IdType id) : base()
        {
            Id = id;
        }
        /// <summary>
        /// The ID of the entity
        /// </summary>
        public IdType Id
        {
            get;
            private set;
        }
    }
}
