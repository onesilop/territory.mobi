using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages
{
    public class DirectoryModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DirectoryModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public Cong Cong { get; set; }
        public IList<Map> Map { get; set; }
        public IList<Section> Section { get; set; }

        public async Task<IActionResult> OnGetAsync(String CongName)
        {
            if (CongName == null)
            {
                return NotFound();
            }

            Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongName == CongName);

            if (Cong == null)
            {
                return NotFound();
            }

            Map = await _context.Map.ToListAsync();
            Map = Map.Where(m => m.CongId == Cong.CongId && m.SectionId == null).OrderBy(m => m.SortOrder).ToList();

            Section = await _context.Section.Where(s => s.CongId == Cong.CongId).ToListAsync();
            foreach (Section s in Section)
            {
                s.Maps = await _context.Map.Where(m => m.SectionId == s.SectionId).ToListAsync();
            }
            return Page();
    }
    }
}
