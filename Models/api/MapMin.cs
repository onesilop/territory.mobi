using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace territory.mobi.Models
{

    [NotMapped]
    public partial class MapMin
    {

        public MapMin(Map m, TerritoryContext context)
        {
            TerritoryContext _context = context; 
            DoNotCalls = new HashSet<DoNotCall>();
            Display = m.Display;
            DoNotCalls =  _context.DoNotCall.Where(x => x.MapId == m.MapId && x.Display == true).ToList();
            ImgImage =  _context.Images.Where(i => i.ImgId == m.ImgId).Select(i => i.ImgImage).SingleOrDefault();
            MapArea = m.MapArea;
            MapKey = m.MapKey;
            Notes = m.Notes;
            Parking = m.Parking;
            Section =  _context.Section.Where(s => s.SectionId == m.SectionId).Select(s => s.SectionTitle).SingleOrDefault();
            SortOrder = m.SortOrder;
            UpdateDatetime = m.UpdateDatetime;
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
