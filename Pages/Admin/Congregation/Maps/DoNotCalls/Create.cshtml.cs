using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.DoNotCalls
{
    public class CreateModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public CreateModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DoNotCall = new DoNotCall();
            DoNotCall.Map = await _context.Map.FirstOrDefaultAsync(m => m.MapId == id).ConfigureAwait(false);

            return Page();
        }

        [BindProperty]
        public DoNotCall DoNotCall { get; set; }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {

            DoNotCall.DncId = Guid.NewGuid();
            DoNotCall.MapId = id;
            DoNotCall.UpdateDatetime = DateTime.UtcNow;
            DoNotCall.DateCreated = DateTime.Now;
            DoNotCall.DateValidated = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.DoNotCall.Add(DoNotCall);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", id.ToString()}
            };
            return RedirectToPage("/Admin/Congregation/Maps/Edit", args);
        }
    }
}