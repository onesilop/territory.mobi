using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Sections
{
    public class DetailsModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DetailsModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public Models.Section Section { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Section = await _context.Section.FirstOrDefaultAsync(m => m.SectionId == id);

            if (Section == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
