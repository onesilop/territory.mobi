using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace territory.mobi.Models
{

[NotMapped]
    public partial class MapDir
    {
        
        public MapDir(Map m)
        {
            MapArea = m.MapArea;
            MapKey = m.MapKey;
            MapId = m.MapId;
        }

        public string MapKey { get; set; }
        public string MapArea { get; set; }
        public Guid MapId { get; set; }

    }
}
