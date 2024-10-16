using AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Dtos
{
    public class EntryDto : EntityDto<string>
    {
        /// <summary>
        /// Creates a new entry
        /// </summary>
        /// <param name="getNewId"></param>
        public EntryDto(Func<string> getNewId) : base(getNewId)
        {
        }

        /// <summary>
        /// Gets the short form of the entry, for example CD
        /// for compact disk
        /// </summary>
        public string Name
        {
            get
            {
                return Id;
            }
        }

        /// <summary>
        /// The description of the entry
        /// </summary>
        public List<string> Description
        {
            get;
            set;
        } = new List<string>();

        /// <summary>
        /// The reference links
        /// </summary>
        public List<LinkDto> ReferenceLinks
        {
            get;
            set;
        } = new List<LinkDto>();

        /// <summary>
        /// The markdown reference link
        /// </summary>
        public string MarkdownReferenceLink
        {
            get;
            set;
        } = string.Empty;
    }
}
