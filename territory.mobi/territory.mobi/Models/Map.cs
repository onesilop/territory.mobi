using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace territory.mobi.Models
{
    public partial class Map
    {
        public Map()
        {
            DoNotCall = new HashSet<DoNotCall>();
            Images = new HashSet<Images>();
        }

        public Guid MapId { get; set; }
        public Guid CongId { get; set; }
        [Display(Name = "Map Key")]
        public string MapKey { get; set; }
        [Display(Name = "Description - Appears at left top")]
        public string MapDesc { get; set; }
        [Display(Name = "Area - Appears at right top")]
        public string MapArea { get; set; }
        public string MapPolygon { get; set; }
        public string MapType { get; set; }
        public Guid? ImgId { get; set; }
        public string GoogleRef { get; set; }
        public string Notes { get; set; }
        public string Parking { get; set; }
        public Guid? SectionId { get; set; }
        [Display(Name = "Order of Apperance")]
        public int SortOrder { get; set; }
        [Display(Name = "Show this map?")]
        public int Display { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public virtual ICollection<DoNotCall> DoNotCall { get; set; }
        public virtual ICollection<Images> Images { get; set; }
    }
}
