using AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Dtos
{
    public class AbbreviationDto : EntityDto<string>
    {
        /// <summary>
        /// Creates a new abbreviation
        /// </summary>
        /// <param name="getNewId"></param>
        public AbbreviationDto(Func<string> getNewId) : base(getNewId)
        {
        }

        /// <summary>
        /// Gets the short form of the abbreviation, for example CD
        /// for compact disk
        /// </summary>
        public string ShortForm
        {
            get
            {
                return Id;
            }
        }

        /// <summary>
        /// Gets the long form of the abbreviation, for example
        /// compact disk for CD
        /// </summary>
        public string LongForm
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// The description of the abbreviation
        /// </summary>
        public string Description
        {
            get;
            set;
        } = string.Empty;

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
