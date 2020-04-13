using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Sections
{
    public class EditModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public EditModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Section Section { get; set; }
        public IList<Map> Maps { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Section = await _context.Section.FirstOrDefaultAsync(m => m.SectionId == id);

            if (Section == null)
            {
                return NotFound();
            }
            Cong ThisCong = await _context.Cong.Where(a => a.CongId == Section.CongId).FirstOrDefaultAsync();
            ViewData["Cong"] = ThisCong.CongName;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Section).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionExists(Section.SectionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "id", Section.CongId.ToString() }
            };
            return RedirectToPage("./Index",args);
        }

        private bool SectionExists(Guid id)
        {
            return _context.Section.Any(e => e.SectionId == id);
        }
    }
}
