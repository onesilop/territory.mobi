using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace territory.mobi.Models
{
    public partial class MapMin
    {

        
        public MapMin()
        {
            DoNotCalls = new HashSet<DoNotCall>();
        }

        public string MapKey { get; set; }
        public string MapDesc { get; set; }
        public string MapArea { get; set; }
        public string Notes { get; set; }
        public string Parking { get; set; }
        public string Section { get; set; }
        public int SortOrder { get; set; }
        public Boolean Display { get; set; }
        public DateTime UpdateDatetime { get; set; }
        public byte[] ImgImage { get; set; }

        public virtual ICollection<DoNotCall> DoNotCalls { get; set; }
    }
}
