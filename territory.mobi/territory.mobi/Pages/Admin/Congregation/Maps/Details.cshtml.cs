using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Maps
{
    public class DetailsModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DetailsModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public Map Map { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Map = await _context.Map.FirstOrDefaultAsync(m => m.MapId == id);

            if (Map == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
