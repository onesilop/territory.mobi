using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Users
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
        public AspNetUsers AspNetUsers { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AspNetUsers = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

            if (AspNetUsers == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AspNetUsers = await _context.AspNetUsers.FindAsync(id);

            if (AspNetUsers != null)
            {
                _context.AspNetUsers.Remove(AspNetUsers);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}
