using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;
using static System.Net.WebRequestMethods;

namespace territory.mobi.Pages
{
    public class mapModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public mapModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Map Map { get; set; }
        public Cong Cong { get; set; }
        public Images Image { get; set; }
        public IList<DoNotCall> DNC { get; set; }

        public async Task<IActionResult> OnGetAsync(string CongName, string MapNo)
        {
            if (MapNo == null)
            {
                return NotFound();
            }

            if (CongName == null)
            {
                return NotFound();
            }

            Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongName == CongName);
            if (Cong == null)
            {
                return NotFound();
            }

            Map = await _context.Map.FirstOrDefaultAsync(m => m.MapKey == MapNo && m.CongId == Cong.CongId);
            if (Map == null)
            {
                return NotFound();
            }

            Image = await _context.Images.FirstOrDefaultAsync(m => m.ImgId == Map.ImgId);

            DNC = await _context.DoNotCall.ToListAsync();
            DNC = DNC.Where(d => d.MapId == Map.MapId).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Map).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(Map.MapId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MapExists(Guid id)
        {
            return _context.Map.Any(e => e.MapId == id);
        }
    }
}
