using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation.Sections
{
    public class IndexModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public IndexModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public IList<Models.Section> Section { get;set; }

        public async Task OnGetAsync(Guid id)
        {
            if (id == null) { NotFound(); }
            Cong ThisCong = await _context.Cong.Where(a => a.CongId == id).FirstOrDefaultAsync().ConfigureAwait(false);
            ViewData["Cong"] = ThisCong.CongName;
            Section = await _context.Section.Where(a => a.CongId == ThisCong.CongId).OrderBy(a => a.SortOrder).ToListAsync().ConfigureAwait(false);
        }
    }
}
