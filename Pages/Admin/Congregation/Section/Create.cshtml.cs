using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace territory.mobi.Pages.Admin.Congregation.Sections
{
    public class CreateModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public CreateModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Section Section { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Models.Section Section = new Models.Section();
            if (id == null) { return NotFound(); }
            Section.CongId = id;
            return Page();

        }



        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Section.CongId = id;
            Section.SectionId = Guid.NewGuid();
            _context.Section.Add(Section);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", Section.CongId.ToString() }
            };
            return RedirectToPage("./Index", args);
        }
    }
}