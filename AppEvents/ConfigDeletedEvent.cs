namespace AppEvents
{
    /// <summary>
    /// Config deleted
    /// </summary>
    public class ConfigDeletedEvent
    {
        /// <summary>
        /// Config deleted
        /// </summary>
        /// <param name="config">The config deleted</param>
        public ConfigDeletedEvent(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// The ID of the config
        /// </summary>
        public Guid Id { get; set; }
    }
}
