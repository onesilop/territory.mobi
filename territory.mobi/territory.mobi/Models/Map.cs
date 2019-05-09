using System;
using System.Collections.Generic;

namespace territory.mobi.Models
{
    public partial class Map
    {
        public Guid MapId { get; set; }
        public Guid CongId { get; set; }
        public string MapKey { get; set; }
        public string MapDesc { get; set; }
        public string MapArea { get; set; }
        public string MapPolygon { get; set; }
        public string MapType { get; set; }
        public Guid? ImgId { get; set; }
        public string GoogleRef { get; set; }
        public string Notes { get; set; }
        public string Parking { get; set; }
        public Guid? SectionId { get; set; }
        public int SortOrder { get; set; }
        public int Display { get; set; }
        public DateTime UpdateDatetime { get; set; }
    }
}
