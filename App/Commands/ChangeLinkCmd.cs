using App.IEntities;

namespace App.Commands
{
    /// <summary>
    /// Change link command
    /// </summary>
    public class ChangeLinkCmd : IReadableLink
    {
        /// <summary>
        /// The URL of the link
        /// </summary>
        public string Url
        {
            get;
            set;
        } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string LinkText
        {
            get;
            set;
        } = string.Empty;
    }
}
