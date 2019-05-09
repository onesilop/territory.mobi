using System;
using System.Collections.Generic;

namespace territory.mobi.Models
{
    public partial class Section
    {

        public Guid SectionId { get; set; }
        public Guid CongId { get; set; }
        public string SectionTitle { get; set; }
        public int SortOrder { get; set; }
        public IList<Map> Maps { get; set; }

    }
}
