using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using territory.mobi.Models;

namespace territory.mobi.Pages.Admin.Congregation
{
    public class IndexModel : PageModel
    {

        private readonly territory.mobi.Models.TerritoryContext _context;

        public IndexModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public IList<Cong> Cong { get; set; }

        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                Response.Redirect("/Admin/Index");
            }
            Cong = await _context.Cong.ToListAsync();
        }
    }
}
