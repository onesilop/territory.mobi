using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DeleteModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cong Cong { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return this.Redirect("/Admin/Index");
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongId == id).ConfigureAwait(false);

                if (Cong == null)
                {
                    return NotFound();
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cong = await _context.Cong.FindAsync(id);

            if (Cong != null)
            {
                _context.Cong.Remove(Cong);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}
