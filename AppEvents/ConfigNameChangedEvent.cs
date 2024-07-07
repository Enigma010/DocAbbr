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
        /// <param name="config">The configuration</param>
        /// <param name="oldName">The old name</param>
        /// <param name="newName">The new name</param>
        /// <param name="oldEnabled">The old enabled</param>
        /// <param name="newEnabled">The new enabled</param>
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
