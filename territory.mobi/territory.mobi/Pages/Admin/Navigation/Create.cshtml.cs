using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Navigation
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
        public tmPage tmPage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.tmPage.Add(tmPage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}