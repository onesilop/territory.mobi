using System;
using System.ComponentModel.DataAnnotations.Schema;

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
