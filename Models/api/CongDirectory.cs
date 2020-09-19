using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace territory.mobi.Models
{
    [NotMapped]
    public partial class CongDirectory
    {


        public CongDirectory(Guid CongId, TerritoryContext context)
        {
            Sections = new HashSet<Section>();
            TerritoryContext _context = context;
            foreach (Section sc in _context.Section.Where(s => s.CongId == CongId).OrderBy(s => s.SortOrder).ToList())
            {
                Section ns = new Section
                {
                    CongId = CongId,
                    SectionId = sc.SectionId,
                    SectionTitle = sc.SectionTitle,
                    SortOrder = sc.SortOrder
                };
                foreach (Map mp in _context.Map.Where(m => m.CongId == CongId && m.SectionId == sc.SectionId).OrderBy(m => m.SortOrder).ToList())
                {
                    ns.MapDirs.Add(new MapDir(mp));
                }
                Sections.Add(ns);
            }
            if (_context.Map.Count(m => m.CongId == CongId && m.SectionId == null) > 0)
            {
                Section sc = new Section
                {
                    CongId = CongId,
                    SectionTitle = "",
                    SortOrder = 999999
                };
                foreach (Map mp in _context.Map.Where(m => m.CongId == CongId && m.SectionId == null).OrderBy(m => m.SortOrder).ToList())
                {
                    sc.MapDirs.Add(new MapDir(mp));
                }
                Sections.Add(sc);
            }
        }

        public virtual ICollection<Section> Sections { get; set; }
    }
}
