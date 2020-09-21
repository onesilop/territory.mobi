using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.DoNotCalls
{
    public class DeleteModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DeleteModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DoNotCall DoNotCall { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DoNotCall = await _context.DoNotCall.FirstOrDefaultAsync(m => m.DncId == id).ConfigureAwait(false);

            if (DoNotCall == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            Guid mpid = DoNotCall.MapId;
            if (id == null)
            {
                return NotFound();
            }

            DoNotCall = await _context.DoNotCall.FindAsync(id);

            if (DoNotCall != null)
            {
                _context.DoNotCall.Remove(DoNotCall);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", mpid.ToString()}
            };
            return RedirectToPage("/Admin/Congregation/Maps/Edit", args);
        }
    }
}
