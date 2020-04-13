using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.DoNotCalls
{
    public class EditModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public EditModel(territory.mobi.Models.TerritoryContext context)
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

            DoNotCall = await _context.DoNotCall.Include(m => m.Map).FirstOrDefaultAsync(m => m.DncId == id);

            if (DoNotCall == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            DoNotCall.UpdateDatetime = DateTime.UtcNow;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(DoNotCall).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoNotCallExists(DoNotCall.DncId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Guid mpid = DoNotCall.MapId;
            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", mpid.ToString()}
            };
            return RedirectToPage("/Admin/Congregation/Maps/Edit", args);
        }

        private bool DoNotCallExists(Guid id)
        {
            return _context.DoNotCall.Any(e => e.DncId == id);
        }
    }
}
