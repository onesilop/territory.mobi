using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Navigation
{
    public class DeleteModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DeleteModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public tmPage tmPage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tmPage = await _context.tmPage.FirstOrDefaultAsync(m => m.PageId == id);

            if (tmPage == null)
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

            tmPage = await _context.tmPage.FindAsync(id);

            if (tmPage != null)
            {
                _context.tmPage.Remove(tmPage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
