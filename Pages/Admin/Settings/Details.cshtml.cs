using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Settings
{
    public class DetailsModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DetailsModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public Setting Setting { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Setting = await _context.Setting.FirstOrDefaultAsync(m => m.SettingId == id).ConfigureAwait(false);

            if (Setting == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
