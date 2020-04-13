using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace territory.mobi.Models
{
    public partial class Section
    {
        public Section()
        {
            Maps = new List<Map>();
            MapDirs = new List<MapDir>();
        }

        public Guid SectionId { get; set; }
        public Guid CongId { get; set; }
        [Display(Name = "Section")]
        public string SectionTitle { get; set; }
        [Display(Name = "Display Order")]
        public int SortOrder { get; set; }
        public IList<Map> Maps { get; set; }
        public IList<MapDir> MapDirs { get; set; }

    }
}
