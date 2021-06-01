using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Maps
{
    public class DeleteModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DeleteModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Map Map { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Map = await _context.Map.FirstOrDefaultAsync(m => m.MapId == id).ConfigureAwait(false);

            if (Map == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            string cngid = "";

            Map = await _context.Map.FindAsync(id);

            if (Map != null)
            {

                cngid = Map.CongId.ToString();
                foreach (Images i in _context.Images.Where(m => m.MapId == Map.MapId).ToList())
                {
                    _context.Images.Remove(i);
                }
                foreach (DoNotCall d in _context.DoNotCall.Where(m => m.MapId == Map.MapId).ToList())
                {
                    _context.DoNotCall.Remove(d);
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);
                _context.Map.Remove(Map);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", cngid}
            };
            return RedirectToPage("/Admin/Congregation/Edit", args);
        }
    }
}
