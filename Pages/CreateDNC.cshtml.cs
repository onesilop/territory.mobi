using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using territory.mobi.Models;

namespace territory.mobi.Pages.CreateDNC
{
    public class CreateModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public CreateModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
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
            DoNotCall.Display = false;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _ = _context.DoNotCall.Add(DoNotCall);
            _ = await _context.SaveChangesAsync().ConfigureAwait(false);
            Mailer m = new Mailer(_context);
            m.SendNewDNC(DoNotCall.DncId, id);
            TempData["UserMessage"] = $"The do not call has been sent to the territory servant for review.";
            TempData["UserMessageClass"] = "alert-success";
            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", id.ToString()}
            };
            return RedirectToPage("/Map", args);
        }
    }
}