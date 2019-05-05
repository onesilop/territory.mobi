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
    public class DetailModel : PageModel
    {
        private readonly territory.mobi.Models.TerritoryContext _context;

        public DetailModel(territory.mobi.Models.TerritoryContext context)
        {
            _context = context;
        }

        public Cong Cong { get; set; }

        public async Task<IActionResult> OnGetAsync(String CongName)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return this.Redirect("/Admin/Index");
            }
            else { 
                if (CongName == null)
                {
                    return NotFound();
                }

                Cong = await _context.Cong.FirstOrDefaultAsync(m => m.CongName == CongName);
            
                if (Cong == null)
                {
                    return NotFound();
                }
                return Page();
            }
        }
    }
}
