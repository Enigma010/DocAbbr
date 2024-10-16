namespace AppEvents
{
    /// <summary>
    /// Config changed state
    /// </summary>
    public class ConfigNameChangedEvent
    {
        /// <summary>
        /// Config state changed
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="oldName">The old name</param>
        /// <param name="newName">The new name</param>
        public ConfigNameChangedEvent(
            Guid id,
            string oldName,
            string newName)
        {
            Id = id;
            OldName = oldName;
            NewName = newName;
        }

        /// <summary>
        /// The ID of the config
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        
        /// <summary>
        /// The old name
        /// </summary>
        public string OldName { get; set; } = string.Empty;
        
        /// <summary>
        /// The new name
        /// </summary>
        public string NewName { get; set; } = string.Empty;
    }
}
