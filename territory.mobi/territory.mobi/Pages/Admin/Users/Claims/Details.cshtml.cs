using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users.Claims
{
    public class DetailsModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DetailsModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public AspNetUserClaims AspNetUserClaims { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AspNetUserClaims = await _context.AspNetUserClaims
                .Include(a => a.User).FirstOrDefaultAsync(m => m.Id == id);

            if (AspNetUserClaims == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
